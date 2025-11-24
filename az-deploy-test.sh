#!/usr/bin/env bash
set -euo pipefail

# === Defaults (edit these for local testing) ===
RESOURCE_GROUP="rg-quickscan"
TEMPLATE_FILE="infra/main.bicep"
WEBAPP_NAME="devopsquickscan"
DOCKERHUB_USERNAME="containers@duijzer.com"
DOCKERHUB_TOKEN="F3EzzY8w9LGvc9"
DOMAIN_NAME="quickscan.duijzer.com"

# === Allow overriding via command-line args ===
# Usage: ./test-deploy.sh <resourceGroup> <webAppName> <dockerUser> <dockerToken> <domain>
if [ $# -ge 1 ]; then RESOURCE_GROUP="$1"; fi
if [ $# -ge 2 ]; then WEBAPP_NAME="$2"; fi
if [ $# -ge 3 ]; then DOCKERHUB_USERNAME="$3"; fi
if [ $# -ge 4 ]; then DOCKERHUB_TOKEN="$4"; fi
if [ $# -ge 5 ]; then DOMAIN_NAME="$5"; fi

echo
echo "======================================================"
echo " Azure Bicep Test Deployment (Resource Group scope)"
echo "======================================================"
echo " Resource Group   : $RESOURCE_GROUP"
echo " Template File    : $TEMPLATE_FILE"
echo " WebApp Name      : $WEBAPP_NAME"
echo " DockerHub User   : $DOCKERHUB_USERNAME"
echo " Domain Name      : $DOMAIN_NAME"
echo

# === Run what-if ===
#az deployment group what-if \
#  --resource-group "$RESOURCE_GROUP" \
#  --template-file "$TEMPLATE_FILE" \
#  --parameters \
#      webAppName="$WEBAPP_NAME" \
#      dockerHubPassword="$DOCKERHUB_TOKEN" \
#      dockerHubUsername="$DOCKERHUB_USERNAME" \
#      customDomainName="$DOMAIN_NAME" \
#  --query "{storageAccountName:properties.outputs.storageAccountName.value, storageAccountKey:properties.outputs.storageAccountKey.value}"


# === Run deployment ===
az deployment group create \
  --resource-group "$RESOURCE_GROUP" \
  --template-file "$TEMPLATE_FILE" \
  --parameters \
      webAppName="$WEBAPP_NAME" \
      dockerHubPassword="$DOCKERHUB_TOKEN" \
      dockerHubUsername="$DOCKERHUB_USERNAME" \
      customDomainName="$DOMAIN_NAME" \
  --query "{storageAccountName:properties.outputs.storageAccountName.value, storageAccountKey:properties.outputs.storageAccountKey.value}" \
#   --debug