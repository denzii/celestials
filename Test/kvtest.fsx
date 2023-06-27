// short test script to test the value coming from keyvault

#r "nuget: Azure.Identity"
#r "nuget: Azure.Security.KeyVault.Secrets"

open Azure.Identity
open Azure.Security.KeyVault.Secrets
open System

let keyVaultUrl = Uri("https://celestials-key-vault.vault.azure.net/")
let credential = DefaultAzureCredential()
let client = SecretClient(keyVaultUrl, credential)
let secretName = "celestials-prod-db-connection"
let secretResponse = client.GetSecret(secretName).Value

let secretValue =
    match secretResponse with
    | null -> "Secret value not found"
    | secret -> secret.Value

printfn "%s" secretValue
