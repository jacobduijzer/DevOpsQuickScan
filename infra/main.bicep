targetScope = 'resourceGroup'

@description('Location for all resources')
param location string = resourceGroup().location

@description('The name of the Web App')
param webAppName string

@description('The name of the App Service plan')
param hostingPlanName string = '${webAppName}-plan'

@description('The name of the Cosmos DB account')
param cosmosDbAccountName string

@description('The name of the Cosmos DB database')
param cosmosDbDatabaseName string = 'questionnaireDb'

@description('The name of the Cosmos DB container')
param cosmosDbContainerName string = 'sessions'

@description('Docker hub password')
param dockerHubPassword string 

@description('Docker hub username')
param dockerHubUsername string 


var appConfigNew = {
  DOCKER_ENABLE_CI: 'true'
  DOCKER_REGISTRY_SERVER_PASSWORD: dockerHubPassword
  DOCKER_REGISTRY_SERVER_URL: 'https://index.docker.io/v1/'
  DOCKER_REGISTRY_SERVER_USERNAME: dockerHubUsername
}

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

resource webApp 'Microsoft.Web/sites@2024-04-01' = {
  name: webAppName
  location: location
  kind: 'app,linux,container'
  properties: {
    serverFarmId: hostingPlan.id
    siteConfig: {
      linuxFxVersion: 'DOCKER|${dockerHubUsername}/devops-quickscan:latest'
    }
  }
}

resource appSettings 'Microsoft.Web/sites/config@2024-04-01' = {
  name: 'appsettings'
  parent: webApp
  properties: appConfigNew
}

// resource cosmosDb 'Microsoft.DocumentDB/databaseAccounts@2021-04-15' = {
//   name: cosmosDbAccountName
//   location: location
//   kind: 'GlobalDocumentDB'
//   properties: {
//     databaseAccountOfferType: 'Standard'
//     locations: [
//       {
//         locationName: location
//         failoverPriority: 0
//       }
//     ]
//     consistencyPolicy: {
//       defaultConsistencyLevel: 'Session'
//     }
//     capabilities: []
//   }
// }

// resource cosmosDbDatabase 'Microsoft.DocumentDB/databaseAccounts/sqlDatabases@2021-04-15' = {
//   name: '${cosmosDb.name}/${cosmosDbDatabaseName}'
//   properties: {
//     resource: {
//       id: cosmosDbDatabaseName
//     }
//   }
//   dependsOn: [cosmosDb]
// }

// resource cosmosDbContainer 'Microsoft.DocumentDB/databaseAccounts/sqlDatabases/containers@2021-04-15' = {
//   name: '${cosmosDb.name}/${cosmosDbDatabase.name}/${cosmosDbContainerName}'
//   properties: {
//     resource: {
//       id: cosmosDbContainerName
//       partitionKey: {
//         paths: ['/sessionId']
//         kind: 'Hash'
//       }
//     }
//   }
//   dependsOn: [cosmosDbDatabase]
// }

output webAppUrl string = 'https://${webAppName}.azurewebsites.net/'
