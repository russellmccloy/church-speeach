targetScope = 'subscription'

param location string // = resourceGroup().location
param prefix string // = resourceGroup().location
param regionCode string 
param environment string 
param suffix string 

var preName = format('{0}-{1}-{2}', prefix , regionCode, environment)


resource churchResourceGroup 'Microsoft.Resources/resourceGroups@2024-03-01' = {
  name: format('{0}-{1}-rg', preName , suffix)
  location: location
}

module appServicePlan './modules/appServicePlan.bicep' = {
  name: 'appServicePlan'
  params: {
    appServicePlanName: format('{0}-{1}-asp', preName , suffix)
    location: churchResourceGroup.location
  }
  scope: churchResourceGroup
}

module appService './modules/appService.bicep' = {
  name: 'appService'
  params: {
    appServiceName: format('{0}-{1}-app', preName , suffix)
    location: churchResourceGroup.location
    appServicePlanId: appServicePlan.outputs.id
  }
  scope: churchResourceGroup
}

module aiSpeechService './modules/aiSpeechService.bicep' = {
  name: 'aiSpeechService'
  params: {
    speechServiceName: format('{0}-{1}-spk', preName , suffix)
    location: churchResourceGroup.location
  }
  scope: churchResourceGroup
}

// module openAIService './modules/openAIService.bicep' = {
//   name: 'openAIService'
//   params: {
//     openAIServiceName: format('{0}-{1}-oai', preName , suffix)
//     location: churchResourceGroup.location
//   }
//   scope: churchResourceGroup
// }
