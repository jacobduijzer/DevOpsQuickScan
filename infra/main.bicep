targetScope = 'resourceGroup'

@description('Location for all resources')
param location string = resourceGroup().location

@description('The name of the Web App')
param webAppName string

@description('The name of the App Service plan')
param hostingPlanName string = '${webAppName}-plan'

@description('Name of the Application Insights instance')
param appInsightsName string = '${webAppName}-appinsights'

@description('Docker hub password')
@secure()
param dockerHubPassword string 

@description('Docker hub username')
param dockerHubUsername string 

var appConfigNew = {
  DOCKER_ENABLE_CI: 'true'
  DOCKER_REGISTRY_SERVER_PASSWORD: dockerHubPassword
  DOCKER_REGISTRY_SERVER_URL: 'https://index.docker.io/v1/'
  DOCKER_REGISTRY_SERVER_USERNAME: dockerHubUsername
  APPINSIGHTS_INSTRUMENTATIONKEY: appInsights.properties.InstrumentationKey
  APPLICATIONINSIGHTS_CONNECTION_STRING: appInsights.properties.ConnectionString
  ASPNETCORE_URLS: 'http://+:8080'
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

resource appInsights 'Microsoft.Insights/components@2020-02-02' = {
  name: appInsightsName
  location: location
  kind: 'web'
  properties: {
    Application_Type: 'web'
  }
}



output webAppUrl string = 'https://${webAppName}.azurewebsites.net/'
