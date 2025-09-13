param appName string = 'azure-function-1'
param location string = resourceGroup().location
var functionAppName = appName
var hostingPlanName = '${appName}-plan'
var storageAccountName = replace('${appName}storage', '-', '')

resource storageAccount 'Microsoft.Storage/storageAccounts@2023-01-01' = {
  name: storageAccountName
  location: location
  sku: {
    name: 'Standard_LRS'
  }
  kind: 'StorageV2'
  properties: {
    supportsHttpsTrafficOnly: true
  }
}

// Create a Consumption plan (default for Function Apps)
resource appServicePlan 'Microsoft.Web/serverfarms@2022-09-01' = {
  name: hostingPlanName
  location: location
  sku: {
    name: 'Y1'
    tier: 'Dynamic'
  }
  kind: 'functionapp'
}
resource functionApp 'Microsoft.Web/sites@2022-09-01' = {
  name: uniqueString(resourceGroup().id, functionAppName, '-web')
  location: location
  kind: 'functionapp'
  identity: {
    type: 'SystemAssigned'
  }
  properties: {
    serverFarmId: appServicePlan.id
    siteConfig: {
      appSettings: [
        {
          name: 'AzureWebJobsStorage'
          value: storageAccount.properties.primaryEndpoints.blob
        }
        {
          name: 'FUNCTIONS_EXTENSION_VERSION'
          value: '~4'
        }
        {
          name: 'FUNCTIONS_WORKER_RUNTIME'
          value: 'dotnet-isolated'
        }
      ]
    }
    httpsOnly: true
  }
}

// Output the Function App URL
output functionAppDefaultHostName string = functionApp.properties.defaultHostName
