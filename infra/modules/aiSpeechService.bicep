// aiSpeechService.bicep
param speechServiceName string
param location string = resourceGroup().location
param skuName string = 'S0' // Default to standard tier

resource speechService 'Microsoft.CognitiveServices/accounts@2022-12-01' = {
  name: speechServiceName
  location: location
  kind: 'SpeechServices'
  sku: {
    name: skuName
  }
  properties: {
    apiProperties: {
      qnaApiKey: ''
    }
  }
}
