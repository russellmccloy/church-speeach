// appServicePlan.bicep
param appServicePlanName string
param location string = resourceGroup().location

resource appServicePlan 'Microsoft.Web/serverfarms@2022-03-01' = {
  name: appServicePlanName
  location: location
  sku: {
    name: 'B1' // Basic tier, single instance
    tier: 'Basic'
    size: 'B1'
    capacity: 1
  }
  properties: {
    reserved: false // Windows hosting
  }
}

// Output the ID of the App Service Plan
output id string = appServicePlan.id
