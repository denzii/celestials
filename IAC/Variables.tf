############################################
// vars
variable "resource_group_name" {
  description = "The name of the resource group in which to create the resources."
  type        = string
  default     = "celestials-resource-group"
}
variable "resource_group_name2" {
  description = "The name of the resource group in which to create the resources."
  type        = string
  default     = "celestials-resource-group2"
}
variable "service_plan_name" {
  description = "The name of the service plan."
  type        = string
  default     = "celestials-service-plan"
}
variable "key_vault_name" {
  description = "The name of the key vault."
  type        = string
  default     = "celestials-key-vault"
}
variable "web_app_name" {
  description = "The name of the web app."
  type        = string
  default     = "celestials-web-app"
}
variable "container_registry_name" {
  description = "The name of the container registry."
  type        = string
  default     = "celestialscr"
}
variable "blob_storage_account_name" {
  description = "The name of the blob storage container."
  type        = string
  default     = "celestialsblobstorage"
}
variable "blob_storage_container_name" {
  description = "The name of the blob storage container."
  type        = string
  default     = "celestials-blob-storage-container"
}
variable "db_server_name" {
    description = "The name of the PostgreSQL server."
    type        = string
    default     = "celestials-postgres-db-server"
}

variable "db_name" {
    description = "The name of the PostgreSQL database."
    type        = string
    default     = "celestials-db"
}

variable "keyvault_db_connection_name" {
  description = "The connection string for the database."
  type        = string
  default = "celestials-prod-db-connection"
}


############################################
# secrets
variable "azure_subscription_id" {
  description = "The value of the Azure subscription id."
  type        = string
}
variable "azure_tenant_id" {
  description = "The value of the Azure tenant id."
  type        = string
}

variable "db_admin_username" {
  description = "The username for the database administrator."
  type        = string
}
variable "db_admin_password" {
  description = "The password for the database administrator."
  type        = string
}
variable "azure_app_service_principal_name" {
  description = "The name for the app service principal"
  type        = string
}
variable "azure_app_service_principal_password" {
  description = "The password for the app service principal"
  type        = string
}

