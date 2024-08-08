// openAIService.bicep
param openAIServiceName string
param location string = resourceGroup().location
param skuName string = 'S0' // Default to standard tier

resource openAIService 'Microsoft.CognitiveServices/accounts@2022-12-01' = {
  name: openAIServiceName
  location: location
  kind: 'OpenAI'
  sku: {
    name: skuName
  }
  properties: {
    apiProperties: {
      qnaApiKey: ''
    }
  }
}
