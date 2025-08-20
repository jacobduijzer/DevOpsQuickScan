targetScope = 'resourceGroup'

@description('Location for all resources')
param location string = resourceGroup().location

@description('The name of the Web App')
param webAppName string

@description('The subdomain to bind (e.g., app.example.com)')
param customDomainName string

@description('The name of the App Service plan')
param hostingPlanName string = '${webAppName}-plan'

@description('Name of the Application Insights instance')
param appInsightsName string = '${webAppName}-appinsights'

@description('Docker hub password')
@secure()
param dockerHubPassword string 

@description('Docker hub username')
param dockerHubUsername string 

var baseStorageAccountName = toLower('${webAppName}${uniqueString(resourceGroup().id)}')
var storageAccountName = substring(baseStorageAccountName, 0, 24)
var questionContainerName = 'questiondata'
var sessionContainerName = 'sessiondata'

resource hostingPlan 'Microsoft.Web/serverfarms@2024-04-01' = {
  name: hostingPlanName
  location: location
  sku: {
    name: 'B1'
    tier: 'Basic'
  }
  kind: 'linux'
  properties: {
    reserved: true
  }
}

resource storageAccount 'Microsoft.Storage/storageAccounts@2024-01-01' = {
  name: storageAccountName
  location: location
  sku: {
    name: 'Standard_LRS'
  }
  kind: 'StorageV2'
  properties: {
    accessTier: 'Hot'
    supportsHttpsTrafficOnly: true
  }
}

resource sessionDataBlobContainer 'Microsoft.Storage/storageAccounts/blobServices/containers@2024-01-01' = {
  name: '${storageAccount.name}/default/${questionContainerName}'
  properties: {
    publicAccess: 'None'
  }
}

resource questionDataBlobContainer 'Microsoft.Storage/storageAccounts/blobServices/containers@2024-01-01' = {
  name: '${storageAccount.name}/default/${sessionContainerName}'
  properties: {
    publicAccess: 'None'
  }
}

resource webApp 'Microsoft.Web/sites@2024-04-01' = {
  name: webAppName
  location: location
  kind: 'app,linux,container'
  identity: {
    type: 'SystemAssigned'
  }
  properties: {
    serverFarmId: hostingPlan.id
    httpsOnly: true
    siteConfig: {
      linuxFxVersion: 'DOCKER|${dockerHubUsername}/devops-quickscan:latest'
    }
  }
}

var appConfigNew = {
  DOCKER_ENABLE_CI: 'true'
  DOCKER_REGISTRY_SERVER_PASSWORD: dockerHubPassword
  DOCKER_REGISTRY_SERVER_URL: 'https://index.docker.io/v1/'
  DOCKER_REGISTRY_SERVER_USERNAME: dockerHubUsername
  APPINSIGHTS_INSTRUMENTATIONKEY: appInsights.properties.InstrumentationKey
  APPLICATIONINSIGHTS_CONNECTION_STRING: appInsights.properties.ConnectionString
  BLOB_STORAGE_ACCOUNT_NAME: storageAccount.name
  BLOB_STORAGE_SHARED_ACCESS_KEY: storageAccount.listKeys().keys[0].value
  BLOG_STORAGE_QUESTIONS_CONTAINER_URL: 'https://${storageAccount.name}.blob.core.windows.net/${questionContainerName}'
  BLOG_STORAGE_SESSION_CONTAINER_URL: 'https://${storageAccount.name}.blob.core.windows.net/${sessionContainerName}'
  ASPNETCORE_URLS: 'http://+:8080'
  WEBSITES_PORT: '8080'
}

resource appSettings 'Microsoft.Web/sites/config@2024-04-01' = {
  name: 'appsettings'
  parent: webApp
  properties: appConfigNew
}

resource domainBinding 'Microsoft.Web/sites/hostNameBindings@2024-11-01' = {
  name: customDomainName
  parent: webApp
  properties: {
    siteName: webApp.name
    hostNameType: 'Verified' 
    customHostNameDnsRecordType: 'CName'
    sslState: 'Disabled' // disable, enable in the module
  }
}

module certificateBindings './bindCertificateToHostname.bicep' = {
  name: '${deployment().name}-ssl'
  params: {
    appServicePlanResourceId: hostingPlan.id
    hostname: customDomainName
    location: location
    webAppName: webApp.name
  }
  dependsOn: [
    domainBinding
  ]
}

resource appInsights 'Microsoft.Insights/components@2020-02-02' = {
  name: appInsightsName
  location: location
  kind: 'web'
  properties: {
    Application_Type: 'web'
  }
}

resource storageBlobContributorRole 'Microsoft.Authorization/roleAssignments@2022-04-01' = {
  name: guid(resourceGroup().id, webApp.id, storageAccount.id, 'Storage Blob Data Contributor')
  scope: storageAccount
  properties: {
    roleDefinitionId: subscriptionResourceId('Microsoft.Authorization/roleDefinitions', 'ba92f5b4-2d11-453d-a403-e96b0029c9fe') // Storage Blob Data Contributor
    principalId: webApp.identity.principalId
    principalType: 'ServicePrincipal'
  }
}

output webAppUrl string = 'https://${webAppName}.azurewebsites.net/'
output storageAccountName string = storageAccount.name
output storageAccountKey string = storageAccount.listKeys().keys[0].value
