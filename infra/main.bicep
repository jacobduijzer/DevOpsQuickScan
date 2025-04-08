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

resource hostingPlan 'Microsoft.Web/serverfarms@2022-03-01' = {
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

resource webApp 'Microsoft.Web/sites@2022-03-01' = {
  name: webAppName
  location: location
  kind: 'app,linux,container'
  properties: {
    serverFarmId: hostingPlan.id
    siteConfig: {
      linuxFxVersion: 'DOCKER|<your-dockerhub-username>/<your-image-name>:latest'
    }
  }
}

resource cosmosDb 'Microsoft.DocumentDB/databaseAccounts@2021-04-15' = {
  name: cosmosDbAccountName
  location: location
  kind: 'GlobalDocumentDB'
  properties: {
    databaseAccountOfferType: 'Standard'
    locations: [
      {
        locationName: location
        failoverPriority: 0
      }
    ]
    consistencyPolicy: {
      defaultConsistencyLevel: 'Session'
    }
    capabilities: []
  }
}

resource cosmosDbDatabase 'Microsoft.DocumentDB/databaseAccounts/sqlDatabases@2021-04-15' = {
  name: '${cosmosDb.name}/${cosmosDbDatabaseName}'
  properties: {
    resource: {
      id: cosmosDbDatabaseName
    }
  }
  dependsOn: [cosmosDb]
}

resource cosmosDbContainer 'Microsoft.DocumentDB/databaseAccounts/sqlDatabases/containers@2021-04-15' = {
  name: '${cosmosDb.name}/${cosmosDbDatabase.name}/${cosmosDbContainerName}'
  properties: {
    resource: {
      id: cosmosDbContainerName
      partitionKey: {
        paths: ['/sessionId']
        kind: 'Hash'
      }
    }
  }
  dependsOn: [cosmosDbDatabase]
}

output webAppUrl string = 'https://${webAppName}.azurewebsites.net/'
