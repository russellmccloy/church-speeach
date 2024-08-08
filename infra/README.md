To deploy your Bicep modules using GitHub Actions, you'll create a workflow file that automates the deployment process. Here's a step-by-step guide on how to set this up:

### 1. **Create a GitHub Actions Workflow**

Inside your repository, you'll need to create a workflow YAML file in the `.github/workflows` directory.

### Example Workflow File

Let's create a workflow that:

1. Checks out your code.
2. Logs in to Azure using a service principal.
3. Deploys the Bicep templates using the Azure CLI.

Create a file called `deploy.yml` in `.github/workflows`:

```yaml
# .github/workflows/deploy.yml
name: Deploy Bicep Templates

on:
  push:
    branches:
      - main  # Trigger deployment on push to the main branch

jobs:
  deploy:
    runs-on: ubuntu-latest

    steps:
      - name: Checkout code
        uses: actions/checkout@v3

      - name: Log in to Azure
        uses: azure/login@v1
        with:
          creds: ${{ secrets.AZURE_CREDENTIALS }}

      - name: Set up Azure CLI
        uses: azure/CLI@v1

      - name: Deploy Bicep templates
        run: |
          az deployment group create \
            --resource-group <your-resource-group> \
            --template-file main.bicep \
            --parameters location=<your-location>

      - name: Log out from Azure
        run: az logout
```

### 2. **Set Up Azure Credentials**

For GitHub Actions to deploy resources to Azure, you'll need to create a service principal and store its credentials as a GitHub secret.

#### Create a Service Principal

Run the following command in the Azure CLI to create a service principal:

```bash
az ad sp create-for-rbac --name "github-actions-deployer" --role contributor --scopes /subscriptions/<your-subscription-id>/resourceGroups/<your-resource-group> --sdk-auth
```

##### Result of above:

```powershell
PS C:\Users\russe\code\canva-openai> az ad sp create-for-rbac --name "github-actions-deployer" --role contributor --scopes /subscriptions/7c3aee63-0f3a-404b-a9de-f784bb35db35 --sdk-auth
Option '--sdk-auth' has been deprecated and will be removed in a future release.
Creating 'contributor' role assignment under scope '/subscriptions/7c3aee63-0f3a-404b-a9de-f784bb35db35'
The output includes credentials that you must protect. Be sure that you do not include these credentials in your code or check the credentials into your source control. For more information, see https://aka.ms/azadsp-cli
{
  "clientId": "6340058c-dc52-422b-a30d-fb8e0753e285",
  "clientSecret": "ITS_A_SECRET",
  "subscriptionId": "7c3aee63-0f3a-404b-a9de-f784bb35db35",
  "tenantId": "574dbe58-968a-4a3a-b963-a15dfe350359",
  "activeDirectoryEndpointUrl": "https://login.microsoftonline.com",
  "resourceManagerEndpointUrl": "https://management.azure.com/",
  "activeDirectoryGraphResourceId": "https://graph.windows.net/",
  "sqlManagementEndpointUrl": "https://management.core.windows.net:8443/",
  "galleryEndpointUrl": "https://gallery.azure.com/",
  "managementEndpointUrl": "https://management.core.windows.net/"
}
```

This command will output a JSON object containing credentials. Copy the entire JSON output.

#### Store the Credentials as a GitHub Secret

1. Go to your GitHub repository.
2. Click on **Settings** > **Secrets and variables** > **Actions**.
3. Click on **New repository secret**.
4. Name the secret `AZURE_CREDENTIALS`.
5. Paste the JSON output from the service principal creation command into the secret's value field and save it.

### 3. **Update Parameters**

In the `deploy.yml` file, replace `<your-resource-group>` and `<your-location>` with the appropriate values for your Azure environment. If your Bicep templates require specific parameters, you can pass them using the `--parameters` option.

### 4. **Push Changes to GitHub**

Once everything is set up:

1. Commit and push your changes to the `main` branch (or the branch you're targeting).
2. The GitHub Actions workflow will automatically trigger and deploy the resources defined in your Bicep files.

### 5. **Monitor the Deployment**

You can monitor the deployment process directly in the **Actions** tab of your GitHub repository. If there are any errors, they will be displayed in the logs, which can help you debug any issues.

### Summary

This GitHub Actions workflow provides an automated way to deploy your Bicep templates to Azure. By using a service principal and GitHub secrets, you ensure that your deployment process is secure and can be easily triggered on code changes.