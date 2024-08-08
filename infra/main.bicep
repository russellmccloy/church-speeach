param location string // = resourceGroup().location

targetScope = 'subscription'

resource churchResourceGroup 'Microsoft.Resources/resourceGroups@2024-03-01' = {
  name: 'churchspeech-ase-dev-rg'
  location: location
}

module appServicePlan './modules/appServicePlan.bicep' = {
  name: 'appServicePlan'
  params: {
    appServicePlanName: 'myAppServicePlan'
    location: churchResourceGroup.location
  }
  scope: churchResourceGroup
}

module appService './modules/appService.bicep' = {
  name: 'appService'
  params: {
    appServiceName: 'myAppService'
    location: churchResourceGroup.location
    appServicePlanId: appServicePlan.outputs.id
  }
  scope: churchResourceGroup
}

module aiSpeechService './modules/aiSpeechService.bicep' = {
  name: 'aiSpeechService'
  params: {
    speechServiceName: 'mySpeechService'
    location: churchResourceGroup.location
  }
  scope: churchResourceGroup
}

module openAIService './modules/openAIService.bicep' = {
  name: 'openAIService'
  params: {
    openAIServiceName: 'myOpenAIService'
    location: churchResourceGroup.location
  }
  scope: churchResourceGroup
}
