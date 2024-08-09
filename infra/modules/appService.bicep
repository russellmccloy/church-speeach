param appServiceName string
param location string = resourceGroup().location
param appServicePlanId string

resource appService 'Microsoft.Web/sites@2022-03-01' = {
  name: appServiceName
  location: location
  properties: {
    serverFarmId: appServicePlanId
    httpsOnly: true
  }
  kind: 'app'
  identity: {
    type: 'SystemAssigned'
  }
}

resource webConfig 'Microsoft.Web/sites/config@2022-03-01' = {
  parent: appService
  name: 'web'
  properties: {
    alwaysOn: false
  }
}
