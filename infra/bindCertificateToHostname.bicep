param webAppName string
param location string
param appServicePlanResourceId string
param hostname string

// Managed certificates can only be created once the hostname is added to the web app.
resource certificates 'Microsoft.Web/certificates@2022-03-01' = { 
  name: '${hostname}-${webAppName}'
  location: location
  properties: {
    serverFarmId: appServicePlanResourceId
    canonicalName: hostname 
  }
}

// sslState and thumbprint can only be set once the managed certificate is created
resource customHostname 'Microsoft.web/sites/hostnameBindings@2019-08-01' = {
  name: '${webAppName}/${hostname}'
  properties: {
    siteName: webAppName
    hostNameType: 'Verified'
    sslState: 'SniEnabled'
    thumbprint: certificates.properties.thumbprint
  }
}
