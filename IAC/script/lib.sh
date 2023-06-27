#!/bin/bash

# Function to print text in blue color
blue() {
  printf '\033[0;34m%s\033[0m\n' "$1"
}
# Function to print text in green color
green() {
  printf '\033[0;32m%s\033[0m\n' "$1"
}
# Function to print text in red color
red() {
  printf '\033[0;31m%s\033[0m\n' "$1"
}

# Function to print text in yellow color
yellow() {
    
  printf '\033[1;33m%s\033[0m\n' "$1"
}


validateEnv(){
    # List of environment variables to check
    variables=(
    AZURE_CELESTIALS_SUBSCRIPTION_ID
    AZURE_CELESTIALS_TENANT_ID
    AZURE_CELESTIALS_RESOURCE_GROUP_NAME
    AZURE_CELESTIALS_SERVICE_PLAN_NAME
    AZURE_CELESTIALS_KEY_VAULT_NAME
    AZURE_CELESTIALS_WEB_APP_NAME
    AZURE_CELESTIALS_CONTAINER_REGISTRY_NAME
    AZURE_CELESTIALS_BLOB_STORAGE_NAME
    AZURE_CELESTIALS_BLOB_STORAGE_CONTAINER_NAME
    AZURE_CELESTIALS_DB_ADMIN_USERNAME
    AZURE_CELESTIALS_DB_ADMIN_PASSWORD
    AZURE_CELESTIALS_DB_SERVER_NAME
    AZURE_DOCKER_REGISTRY_SERVER_USER
    AZURE_DOCKER_REGISTRY_SERVER_PASSWORD
    AZURE_DOCKER_REGISTRY_SERVER_URL
    )

    # Check if any variables are empty
    empty_variables=()
    for variable in "${variables[@]}"; do
    if [[ -z "${!variable}" ]]; then
        empty_variables+=("$variable")
    fi
    done

    # Display empty variables and prompt users to set them
    if [[ ${#empty_variables[@]} -gt 0 ]]; then
    red "The following environment variables are empty:"
    for variable in "${empty_variables[@]}"; do
        red "- $variable"
    done

    red "Please set the empty variables manually or run the following command in the IAC directory to run this deploy script via terraform:"
    yellow "terraform apply -var-file=Secrets.tfvars -target=null_resource.celestials_localhost_provisioner -replace=null_resource.celestials_localhost_provisioner -auto-approve -compact-warnings"
    exit 1
    fi

    # All variables are set
    green "All required environment variables are set."
}

validateAzureCli(){
    # Check if `az` command is available
    if ! command -v az; then
        red "Azure CLI (az) command not found. Please install Azure CLI and try again."
        exit 1
    fi

    # Check if user is already logged in to Azure
    if ! az account show; then
        yellow "Please sign in to your Azure account by clicking on the following url or following to the opened browser screen."
        az login
    fi
    green "Currently using the following Azure subscription:"
    az account show
}

validateNetlifyCli(){
    # check if npm command is availale
    if ! command -v npm; then
        yellow "Npmjs (npm) command not found. Please install npm and try again."
        exit 1
    fi
    # Check if `netlify` command is available
    if ! command -v netlify; then
        yellow "Netlify CLI (netlify) command not found. Please install Netlify CLI via \"npm install -g netlify-cli and try again."
        exit 1
    fi

    # Check if the netlify command fails and execute login if necessary
    if ! netlify sites:list >/dev/null 2>&1; then
        yellow "Please sign in to your Netlify account by clicking on the following URL or navigating to the opened browser screen."
        netlify login
    fi
}

