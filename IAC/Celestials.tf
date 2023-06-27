############################################
// provider setup
data "azuread_client_config" "current" {}
data "azurerm_client_config" "current" {}

provider "azurerm" {
  alias           = "prod"
  subscription_id = var.azure_subscription_id
  tenant_id       = var.azure_tenant_id

  features {}
}

provider "azurerm" {
  alias           = "obo"
  subscription_id = var.azure_subscription_id
  tenant_id       = var.azure_tenant_id

  features {}
}

provider "azurerm" {
  features {}
}

############################################
 // resources
resource "azurerm_resource_group" "celestials_prod_resource_group" {
  name     = var.resource_group_name
  location = "UK South"
}

resource "azurerm_key_vault" "celestials_prod_key_vault" {
  name                        = var.key_vault_name
  location                    = azurerm_resource_group.celestials_prod_resource_group.location
  resource_group_name         = azurerm_resource_group.celestials_prod_resource_group.name
  enabled_for_deployment      = true
  tenant_id                   = var.azure_tenant_id
  sku_name                    = "standard"
}

resource "azurerm_key_vault_access_policy" "celestials_web_app_access_policy_app" {
  provider = azurerm.obo
  key_vault_id       = azurerm_key_vault.celestials_prod_key_vault.id
  tenant_id          = data.azurerm_client_config.current.tenant_id
  object_id          = "6067d0d0-7de1-4da8-bb06-e308d79dc4dd"
  secret_permissions = ["Get", "Set", "List"]
}


resource "azurerm_role_assignment" "celestials_kv_role_assignment" {
  scope                = azurerm_key_vault.celestials_prod_key_vault.id
  role_definition_name = "Key Vault Secrets Officer"
  principal_id         = data.azurerm_client_config.current.object_id
}

resource "azurerm_service_plan" "celestials_prod_service_plan" {
  name                = var.service_plan_name
  location            = azurerm_resource_group.celestials_prod_resource_group.location
  resource_group_name = azurerm_resource_group.celestials_prod_resource_group.name
  sku_name            = "S1"
  os_type             = "Linux"
}

resource "azurerm_linux_web_app" "celestials_prod_web_app" {
  name                = var.web_app_name
  location            = azurerm_resource_group.celestials_prod_resource_group.location
  resource_group_name = azurerm_resource_group.celestials_prod_resource_group.name
  service_plan_id = azurerm_service_plan.celestials_prod_service_plan.id
  site_config {
    always_on = true
    container_registry_use_managed_identity = true
  }
  identity {
    type = "SystemAssigned"
  }
  logs {
    application_logs {
      file_system_level="Information"
    }
    http_logs{
      file_system{
        retention_in_days = 0
        retention_in_mb = 50
      }
    }
  }
  app_settings = {
        DOCKER_REGISTRY_SERVER_USERNAME     = "celestialscr"
        DOCKER_REGISTRY_SERVER_PASSWORD     = var.azure_app_service_principal_password
        DOCKER_REGISTRY_SERVER_URL          = "${azurerm_container_registry.celestials_prod_cr.name}.azurecr.io"
        WEBSITES_ENABLE_APP_SERVICE_STORAGE = false
  }
}
resource "azurerm_role_assignment" "celestials_web_app_kv_access" {
  scope                = azurerm_key_vault.celestials_prod_key_vault.id
  role_definition_name = "Reader"
  principal_id         = azurerm_linux_web_app.celestials_prod_web_app.identity[0].principal_id
}

resource "azurerm_container_registry" "celestials_prod_cr" {
  name                = var.container_registry_name
  location            = azurerm_resource_group.celestials_prod_resource_group.location
  resource_group_name = azurerm_resource_group.celestials_prod_resource_group.name
  sku                 = "Standard"
  admin_enabled       = true
}

resource "azurerm_key_vault_secret" "docker_registry_server_user" {
  name         = "docker-registry-server-user"
  value        = azurerm_container_registry.celestials_prod_cr.admin_username
  key_vault_id = azurerm_key_vault.celestials_prod_key_vault.id
  depends_on   = [azurerm_container_registry.celestials_prod_cr]
}

resource "azurerm_key_vault_secret" "docker_registry_server_password" {
  name         = "docker-registry-server-password"
  value        = azurerm_container_registry.celestials_prod_cr.admin_password
  key_vault_id = azurerm_key_vault.celestials_prod_key_vault.id
  depends_on   = [azurerm_container_registry.celestials_prod_cr]
}

resource "azurerm_storage_account" "celestials_prod_blob_storage_account" {
  name                     = var.blob_storage_account_name
  resource_group_name      = azurerm_resource_group.celestials_prod_resource_group.name
  location                 = azurerm_resource_group.celestials_prod_resource_group.location
  account_tier             = "Standard"
  account_replication_type = "LRS"
  enable_https_traffic_only = true
}



resource "azuread_application" "celestials_aad_app" {
  display_name = "celestials-aad-app"
  owners       = [data.azuread_client_config.current.object_id]
}
resource "azuread_service_principal" "celestials_web_app_service_principal" {
  application_id               = azuread_application.celestials_aad_app.application_id
  app_role_assignment_required = false
  owners                       = [data.azuread_client_config.current.object_id]
  depends_on = [ azuread_application.celestials_aad_app ]
}

resource "azuread_service_principal_password" "celestials_web_app_service_principal_password" {
  service_principal_id = azuread_service_principal.celestials_web_app_service_principal.object_id
}

resource "azurerm_role_assignment" "celestials_principal_aad_acrpull" {
  scope                = "/subscriptions/${var.azure_subscription_id}/resourceGroups/${azurerm_resource_group.celestials_prod_resource_group.name}"
  role_definition_name = "acrpull"
  principal_id         = azurerm_linux_web_app.celestials_prod_web_app.identity[0].principal_id
  depends_on = [ azuread_application.celestials_aad_app, azurerm_linux_web_app.celestials_prod_web_app ]
}

resource "azurerm_storage_container" "celestials_prod_blob_storage_container" {
  name                  = var.blob_storage_container_name
  storage_account_name  = azurerm_storage_account.celestials_prod_blob_storage_account.name
  container_access_type = "blob" // must be private in real world 
}

resource "azurerm_postgresql_server" "celestials_prod_pgdb" {
  name                         = var.db_server_name
  location                     = azurerm_resource_group.celestials_prod_resource_group.location
  resource_group_name          = azurerm_resource_group.celestials_prod_resource_group.name
  sku_name                     = "B_Gen5_1"
  storage_mb                   = 5120
  backup_retention_days        = 7
  administrator_login          = var.db_admin_username
  administrator_login_password = var.db_admin_password
  version                      = 11
  ssl_enforcement_enabled      = true
}

resource "azurerm_postgresql_firewall_rule" "celestials_prod_pgdb_fw" {
  name                = "AllowAppServer"
  resource_group_name = azurerm_resource_group.celestials_prod_resource_group.name
  server_name         = azurerm_postgresql_server.celestials_prod_pgdb.name
  start_ip_address    = "40.81.154.181"
  end_ip_address      = "40.81.154.181"
}

resource "azurerm_postgresql_database" "celestials_prod_pgdb_db" {
  name                = var.db_name
  resource_group_name = azurerm_resource_group.celestials_prod_resource_group.name
  server_name         = var.db_server_name
  charset             = "UTF8"
  collation           = "English_United States.1252"
}

resource "azurerm_key_vault_secret" "celestials_prod_db_connection" {
  name         = var.keyvault_db_connection_name
  value        = "DATABASE_CONNECTION=Server=${var.db_server_name}.postgres.database.azure.com;Database=${var.db_name};Port=5432;User Id=${var.db_admin_username}@${var.db_server_name};Password=${var.db_admin_password};Ssl Mode=VerifyFull"
  key_vault_id = azurerm_key_vault.celestials_prod_key_vault.id
}

############################################
// set env variables on the local machine and run the deploy script
resource "null_resource" "celestials_localhost_provisioner" {
  provisioner "local-exec" {
    command     = "./bin/deploy.sh"
    interpreter = ["/bin/bash"]
    working_dir = path.module
    
    environment = {
      AZURE_CELESTIALS_SUBSCRIPTION_ID             = var.azure_subscription_id
      AZURE_CELESTIALS_TENANT_ID                   = var.azure_tenant_id
      AZURE_CELESTIALS_RESOURCE_GROUP_NAME         = var.resource_group_name
      AZURE_CELESTIALS_SERVICE_PLAN_NAME           = var.service_plan_name
      AZURE_CELESTIALS_KEY_VAULT_NAME              = var.key_vault_name
      AZURE_CELESTIALS_WEB_APP_NAME                = var.web_app_name
      AZURE_CELESTIALS_CONTAINER_REGISTRY_NAME     = var.container_registry_name
      AZURE_CELESTIALS_BLOB_STORAGE_NAME           = var.blob_storage_account_name
      AZURE_CELESTIALS_BLOB_STORAGE_CONTAINER_NAME = var.blob_storage_container_name
      AZURE_CELESTIALS_DB_ADMIN_USERNAME           = var.db_admin_username
      AZURE_CELESTIALS_DB_ADMIN_PASSWORD           = var.db_admin_password
      AZURE_CELESTIALS_DB_SERVER_NAME              = var.db_server_name
      # the below az server secrets are set as non sensitive here
      # this is done to avoid the  suppression of output from deploy.sh
      # its okay as the script itself doesnt display those on the console
      AZURE_DOCKER_REGISTRY_SERVER_USER            = "celestialscr"
      AZURE_DOCKER_REGISTRY_SERVER_PASSWORD        = var.azure_app_service_principal_password
    }
  }
}

