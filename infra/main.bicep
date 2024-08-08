param location string = resourceGroup().location

module appServicePlan './modules/appServicePlan.bicep' = {
  name: 'appServicePlan'
  params: {
    appServicePlanName: 'myAppServicePlan'
    location: location
  }
}

module appService './modules/appService.bicep' = {
  name: 'appService'
  params: {
    appServiceName: 'myAppService'
    location: location
    appServicePlanId: appServicePlan.outputs.id
  }
}

module aiSpeechService './modules/aiSpeechService.bicep' = {
  name: 'aiSpeechService'
  params: {
    speechServiceName: 'mySpeechService'
    location: location
  }
}

module openAIService './modules/openAIService.bicep' = {
  name: 'openAIService'
  params: {
    openAIServiceName: 'myOpenAIService'
    location: location
  }
}
