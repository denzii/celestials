# Celestials (Inclusive Tech)
![Language Count](https://img.shields.io/github/languages/count/denzii/celestials)

## What is included in this project?
* 1- .NET Web Api Backend (Celestials)
* 2- Static React-App with Typescript
* 3- Local IAC using docker-compose and Posgresql
* 4- Cloud IAC using Terraform with the Azure resource manager
* 5- Imperative Deploy toolkit with Bash

## .NET Web Api Backend Highlights:
* 1- Code first migrations & data seeding with EF-Core
* 2- System Abstraction with Docker
* 3- Clean appropriated layered Code architecture
* 4- Unit testing & Integration testing

## React & TS Frontend Highlights:
* 1- Responsive Design
* 2- Semantic HTML5 
* 3- BEM appropriated extensible CSS architecture
* 4- Screen Reader Accessibility with visually hidden description data points
* 5- Caching to avoid making multiple API requests
* 6- E2E Testing suite

## Local Infrastructure Highlights:
* 1- Docker Compose orchestration to avoid local setups on the persistence layer
* 2- Data Volumes for persistent storage

## Cloud Infrastructure Highlights: 
* 1- Complete Infrastructure declaration through Terraform
* 2- Cloud Native with Azure & Netlify
* 3- Secrets hidden/abstracted away using keyvault.

## Devops Toolkit Highlights:
* 1- Full Deploy automation
* 2- Standardized results across environments via terraform local-exec
* 3- Local machine validations for tools such as npm, azure cli etc.
* 4- Colour coded terminal outputs
* 5- Error handling logic to ensure deterministic behaviour without side-effects

### Demo & Documentation
    Frontend: https://celestialsweb.netlify.app/
    Backend : https://celestials-web-app.azurewebsites.net/ 
    -- Endpoints:
    1-GET /Planet/GetAll
    2-GET /Planet/Get?id=1

## Run the backend
 ``` (cd Celestials && docker-compose up) ```

## Run the backend test suite
``` (cd Celestials/Test && dotnet test) ```

## Run the frontend
 ``` (cd Celestials/Web && npm i && npm start) ```

## Run the frontend End to End tests via ElectronJS
 ``` (cd Celestials/Web  && npx cypress open) ```

## Run the frontend End to end tests (headless)
``` npx cypress run --headless ```

## Run the infrastructure
* 1- ``` cd Celestials/IAC ```
* 2- ``` touch Secrets.tfvars ```
- Put the following secrets into Secrets.tfvars via IDE:
      db_admin_username
      db_admin_password
      azure_subscription_id
      azure_tenant_id
      azure_app_service_principal_name
      azure_app_service_principal_password

* 3- ``` terraform apply -var-file=Secrets.tfvars ```

## Rebuild & Deploy All Production Components
``` terraform apply -var-file=Secrets.tfvars -target=null_resource.celestials_localhost_provisioner -replace=null_resource.celestials_localhost_provisioner -auto-approve ```

## Cloud Resources leveraged:
   * Key vault
   * Azure Database for PostgreSQL single server
   * Azure App Service plan
   * Azure App Service
   * Azure Storage account
   * Azure Container registry
   * Netlify Static 


## Author
    - Backend, Frontend, Infrastructure & Devops by [@denzii](https://github.com/denzii)

## Tech Stack

**Client:** Create React App, TypeScript, Styled-Components, HTML5 (CSS3)

**Server:** C#.NET Web API, Entity Framework Core (PGSQL)

**Platform:** Docker, Docker Compose, Bash, Terraform, Azure CLI, Netlify CLI

**Infra:**  Azure, Terraform