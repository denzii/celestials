#!/bin/bash

cd "$(dirname "$0")" && \
source ./lib.sh

# pre-requisite check to short-circuit the script if lib.sh failed to source
if [[ $? -ne 0 ]]; then
    RED='\033[0;31m'
    UNCOLOURED='\033[0m'

    printf "${RED}" && \
    echo "Error: Failed to source lib.sh. Exiting." && \
    printf "${UNCOLOURED}" && \
    exit 1
fi

# local functions for deploying the backend and frontend
deployFrontend(){
    blue "Attempting to bundle & deploy the static frontend assets to Netlify..."
    if [[ -z "$(netlify status | grep 'Error:')" ]]; then
        yellow "Error: You don't appear to be in a folder that is linked to a site. Creating a new site..."

        # pipe a newline into the command so that it will continue to run by an auto answer to choose default team
        # add arithmetic precedence to the command so that we can use fallback true to if site already exists and move on
        ((echo; sleep 1) | netlify sites:create --name celestialsweb) || \
        true && \
        (
            cd ../../Web && \
            netlify link --name celestialsweb && \
            npm i && npm run build && \
            ((echo "./build"; sleep 1) | netlify deploy --prod) && \
            green "Successfully deployed the static frontend assets to Netlify."
        )
    fi 
}
deployBackend() {
    TAG="celestials:latest"
    #Build and upload the container image to Azure Container Registry
    blue "Attempting to build and upload the container image to Azure Container Registry..."
    az acr build -t $TAG -r ${AZURE_CELESTIALS_CONTAINER_REGISTRY_NAME} ../../   
    if [[ $? -eq 0 ]]; then
        green "Build and upload of container image onto acr succeeded."
    else
        red "Attempt to build and upload of container image onto acr failed. Please see the error output from Azure above! Exiting..."
        exit 1
    fi

    #Deploy the web app to Azure App Service
    blue "Attempting to deploy the web app to Azure App Service..."
    az webapp config container set \
        --name ${AZURE_CELESTIALS_WEB_APP_NAME} \
        --resource-group ${AZURE_CELESTIALS_RESOURCE_GROUP_NAME} \
        --docker-custom-image-name ${TAG} \
        --docker-registry-server-url ${AZURE_DOCKER_REGISTRY_SERVER_URL} \
        --docker-registry-server-user ${AZURE_DOCKER_REGISTRY_SERVER_USER} \
        --docker-registry-server-password ${AZURE_DOCKER_REGISTRY_SERVER_PASSWORD};
    blue "Enabling logging to file system..."    

    if [[ $? -eq 0 ]]; then
        green " Deploy of the web app to Azure App Service succeeded."
    else
        red "Attempt to deploy the web app to Azure App Service failed. Please see the error output from Azure above! Exiting..."
        exit 1
    fi
}


# Local deployment pipeline
validateEnv && \
validateAzureCli && \
validateNetlifyCli && \
deployBackend && \
deployFrontend && {
    green "Deployment completed successfully amd should be available shortly."
    green "To view Azure AppService logs for the Backend, run the following command:"
    green "az webapp log tail --name ${AZURE_CELESTIALS_WEB_APP_NAME} --resource-group ${AZURE_CELESTIALS_RESOURCE_GROUP_NAME}";
    green "To open the deployment in a browser, run the following command:"
    green "az webapp browse --name ${AZURE_CELESTIALS_WEB_APP_NAME} --resource-group ${AZURE_CELESTIALS_RESOURCE_GROUP_NAME}";
exit 0;
}

