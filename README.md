# Serverless Ponies 🐴

This repo contains a work-in-progress game I'm building together with my 9 year-old daughter. It's a sample of how to create a serverless web application using Azure Functions on the server-side and Blazor WebAssembly on the client-side (hosted in an Azure Storage static website).

Prerequisites:
- Azure SignalR service
- Azure Functions 




- Open the `src` folder in Visual Studio Online.

If you've got Docker and the [devcontainer plugin] installed, you can use the development container that is part of this repository. Visual Studio Code will ask you if you want to re-open the folder in the development container:

[devcontainer]

Click **Reopen in Container** to let VS Code create the container. This may take some time, and a progress notification will provide status updates.

### Create Azure resources

When the folder has reopened in the development container, open a new Terminal window in VS Code and navigate to the `src/deployment` folder. Use `az login` to log in to your Azure subscription. Then run the `deploy.azcli` script to deploy the required Azure resources (Function App, SignalR Service and a Storage Account).

### Deploy the back-end to Azure Functions

Once the deployment script has completed, right-click the *FunctionApplication* folder and select **Deploy to Function App**. VS Code will then ask you to select a subscription and a Function App. Select the *serverlessponiesxxxx* Function App that was created by the `deploy.azcli` script.

### Deploy the front-end to Azure Storage Static Website

