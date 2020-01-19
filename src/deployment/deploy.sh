az group create -n ServerlessPonies -l WestEurope

az signalr create \
    -n PoniesSignalr \
    -g ServerlessPonies \
    -l WestEurope \
    --sku Standard_S1 \
    --service-mode Serverless