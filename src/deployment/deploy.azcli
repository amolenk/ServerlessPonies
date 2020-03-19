#!/bin/bash

resourceGroupName=ServerlessPoniesTestDeploy
location=WestEurope
storageAccountName=serverlessponies9321
storageAccountKind=StorageV2
storageAccountSku=Standard_LRS
functionAppName=serverlessponies9321
signalrServiceName=serverlessponies9321
signalrServiceSku=Free_F1

az group create -n $resourceGroupName -l $location

az storage account create \
    -n $storageAccountName \
    -g $resourceGroupName \
    --kind $storageAccountKind \
    -l $location \
    --sku $storageAccountSku

# Enable static website feature.
az storage blob service-properties update \
    --account-name $storageAccountName \
    --static-website \
    --index-document index.html

az signalr create \
    -n $signalrServiceName \
    -g $resourceGroupName \
    -l $location \
    --sku $signalrServiceSku \
    --service-mode Serverless

storageAccountId=$(az storage account show -g $resourceGroupName -n $storageAccountName --query id -o tsv)

az functionapp create \
    -n $functionAppName \
    -g $resourceGroupName \
    --consumption-plan-location $location \
    --runtime dotnet \
    --runtime-version 3 \
    --storage-account $storageAccountId

# Add a CORS allowed origin.
storageAccountWebEndpoint=$(az storage account show -g $resourceGroupName -n $storageAccountName --query primaryEndpoints.web -o tsv)
#
az functionapp cors add \
    -n $functionAppName \
    -g $resourceGroupName \
    --allowed-origins $storageAccountWebEndpoint

# Set Function app settings
storageAccountConnectionString=$(az storage account show-connection-string -g $resourceGroupName -n $storageAccountName --query connectionString -o tsv)
signalrServiceConnectionString=$(az signalr key list -g $resourceGroupName -n $signalrServiceName --query primaryKey -o tsv)
signalrServiceHostName=$(az signalr show -g $resourceGroupName -n $signalrServiceName --query hostName -o tsv)
#
az functionapp config appsettings set \
    -n $functionAppName \
    -g $resourceGroupName \
    --settings "AzureWebJobsStorage=$storageAccountConnectionString" "AzureSignalRConnectionString=Endpoint=https://$signalrServiceHostName;AccessKey=$signalrServiceConnectionString;Version=1.0;"



