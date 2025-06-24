#!/usr/bin/env bash
set -euo pipefail

# PROJECT name for reuse
PROJECT="TrackIt.Api.IntegrationTests"

# Load the JSON blob into a variable
JSON="$SERVER_SECRETS_JSON"

# Helper to extract fields via jq
get() {
  echo "$JSON" | jq -r "$1"
}

# 1) Init (can safely fail if already initialized)
dotnet user-secrets init --project $PROJECT || true

# 2) Connection String - handle special case with proper quoting like backslashes
# Use a different connection string for CI/CD
dotnet user-secrets set \
  "ConnectionStrings:TrackIt" \
  "Server=localhost;Database=TrackItDB_test;User ID=sa;Password=YourPassword;TrustServerCertificate=true;MultipleActiveResultSets=true" \
  --project $PROJECT

# 3) DefaultAccounts
dotnet user-secrets set \
  "DefaultAccounts:AdminUserName" \
  "$(get '.DefaultAccounts.AdminUserName')" \
  --project $PROJECT

dotnet user-secrets set \
  "DefaultAccounts:AdminPassword" \
  "$(get '.DefaultAccounts.AdminPassword')" \
  --project $PROJECT

dotnet user-secrets set \
  "DefaultAccounts:AdminEmail" \
  "$(get '.DefaultAccounts.AdminEmail')" \
  --project $PROJECT

# 4) SUPPLIER ACCOUNTS REGISTER FLAG
dotnet user-secrets set \
  "SupplierAccountsRegister" \
  "$(get '.SupplierAccountsRegister')" \
  --project $PROJECT

# Supplier Alpha Account
dotnet user-secrets set \
  "SupplierAccounts:SupplierAlphaUserName" \
  "$(get '.SupplierAccounts.SupplierAlphaUserName')" \
  --project $PROJECT

dotnet user-secrets set \
  "SupplierAccounts:SupplierAlphaPassword" \
  "$(get '.SupplierAccounts.SupplierAlphaPassword')" \
  --project $PROJECT

dotnet user-secrets set \
  "SupplierAccounts:SupplierAlphaFullName" \
  "$(get '.SupplierAccounts.SupplierAlphaFullName')" \
  --project $PROJECT

dotnet user-secrets set \
  "SupplierAccounts:SupplierAlphaEmail" \
  "$(get '.SupplierAccounts.SupplierAlphaEmail')" \
  --project $PROJECT

dotnet user-secrets set \
  "SupplierAccounts:SupplierAlphaJobTitle" \
  "$(get '.SupplierAccounts.SupplierAlphaJobTitle')" \
  --project $PROJECT

dotnet user-secrets set \
  "SupplierAccounts:SupplierAlphaPhoneNumber" \
  "$(get '.SupplierAccounts.SupplierAlphaPhoneNumber')" \
  --project $PROJECT

dotnet user-secrets set \
  "SupplierAccounts:SupplierAlphaConfiguration" \
  "$(get '.SupplierAccounts.SupplierAlphaConfiguration')" \
  --project $PROJECT

dotnet user-secrets set \
  "SupplierAccounts:SupplierAlphaIsEnabled" \
  "$(get '.SupplierAccounts.SupplierAlphaIsEnabled')" \
  --project $PROJECT

# Supplier Beta Account
dotnet user-secrets set \
  "SupplierAccounts:SupplierBetaUserName" \
  "$(get '.SupplierAccounts.SupplierBetaUserName')" \
  --project $PROJECT

dotnet user-secrets set \
  "SupplierAccounts:SupplierBetaPassword" \
  "$(get '.SupplierAccounts.SupplierBetaPassword')" \
  --project $PROJECT

dotnet user-secrets set \
  "SupplierAccounts:SupplierBetaFullName" \
  "$(get '.SupplierAccounts.SupplierBetaFullName')" \
  --project $PROJECT

dotnet user-secrets set \
  "SupplierAccounts:SupplierBetaEmail" \
  "$(get '.SupplierAccounts.SupplierBetaEmail')" \
  --project $PROJECT

dotnet user-secrets set \
  "SupplierAccounts:SupplierBetaJobTitle" \
  "$(get '.SupplierAccounts.SupplierBetaJobTitle')" \
  --project $PROJECT

dotnet user-secrets set \
  "SupplierAccounts:SupplierBetaPhoneNumber" \
  "$(get '.SupplierAccounts.SupplierBetaPhoneNumber')" \
  --project $PROJECT

dotnet user-secrets set \
  "SupplierAccounts:SupplierBetaConfiguration" \
  "$(get '.SupplierAccounts.SupplierBetaConfiguration')" \
  --project $PROJECT

dotnet user-secrets set \
  "SupplierAccounts:SupplierBetaIsEnabled" \
  "$(get '.SupplierAccounts.SupplierBetaIsEnabled')" \
  --project $PROJECT

# Supplier Gamma Account
dotnet user-secrets set \
  "SupplierAccounts:SupplierGammaUserName" \
  "$(get '.SupplierAccounts.SupplierGammaUserName')" \
  --project $PROJECT

dotnet user-secrets set \
  "SupplierAccounts:SupplierGammaPassword" \
  "$(get '.SupplierAccounts.SupplierGammaPassword')" \
  --project $PROJECT

dotnet user-secrets set \
  "SupplierAccounts:SupplierGammaFullName" \
  "$(get '.SupplierAccounts.SupplierGammaFullName')" \
  --project $PROJECT

dotnet user-secrets set \
  "SupplierAccounts:SupplierGammaEmail" \
  "$(get '.SupplierAccounts.SupplierGammaEmail')" \
  --project $PROJECT

dotnet user-secrets set \
  "SupplierAccounts:SupplierGammaJobTitle" \
  "$(get '.SupplierAccounts.SupplierGammaJobTitle')" \
  --project $PROJECT

dotnet user-secrets set \
  "SupplierAccounts:SupplierGammaPhoneNumber" \
  "$(get '.SupplierAccounts.SupplierGammaPhoneNumber')" \
  --project $PROJECT

dotnet user-secrets set \
  "SupplierAccounts:SupplierGammaConfiguration" \
  "$(get '.SupplierAccounts.SupplierGammaConfiguration')" \
  --project $PROJECT

dotnet user-secrets set \
  "SupplierAccounts:SupplierGammaIsEnabled" \
  "$(get '.SupplierAccounts.SupplierGammaIsEnabled')" \
  --project $PROJECT

# 5) FACILITY ACCOUNTS REGISTER FLAG
dotnet user-secrets set \
  "FacilityAccountsRegister" \
  "$(get '.FacilityAccountsRegister')" \
  --project $PROJECT

# Facility One Account
dotnet user-secrets set \
  "FacilityAccounts:FacilityOneUserName" \
  "$(get '.FacilityAccounts.FacilityOneUserName')" \
  --project $PROJECT

dotnet user-secrets set \
  "FacilityAccounts:FacilityOnePassword" \
  "$(get '.FacilityAccounts.FacilityOnePassword')" \
  --project $PROJECT

dotnet user-secrets set \
  "FacilityAccounts:FacilityOneFullName" \
  "$(get '.FacilityAccounts.FacilityOneFullName')" \
  --project $PROJECT

dotnet user-secrets set \
  "FacilityAccounts:FacilityOneEmail" \
  "$(get '.FacilityAccounts.FacilityOneEmail')" \
  --project $PROJECT

dotnet user-secrets set \
  "FacilityAccounts:FacilityOneJobTitle" \
  "$(get '.FacilityAccounts.FacilityOneJobTitle')" \
  --project $PROJECT

dotnet user-secrets set \
  "FacilityAccounts:FacilityOnePhoneNumber" \
  "$(get '.FacilityAccounts.FacilityOnePhoneNumber')" \
  --project $PROJECT

dotnet user-secrets set \
  "FacilityAccounts:FacilityOneConfiguration" \
  "$(get '.FacilityAccounts.FacilityOneConfiguration')" \
  --project $PROJECT

dotnet user-secrets set \
  "FacilityAccounts:FacilityOneIsEnabled" \
  "$(get '.FacilityAccounts.FacilityOneIsEnabled')" \
  --project $PROJECT

# 6) DELIVERY ACCOUNTS REGISTER FLAG
dotnet user-secrets set \
  "DeliveryAccountsRegister" \
  "$(get '.DeliveryAccountsRegister')" \
  --project $PROJECT

# Delivery One Account
dotnet user-secrets set \
  "DeliveryAccounts:DeliveryOneUserName" \
  "$(get '.DeliveryAccounts.DeliveryOneUserName')" \
  --project $PROJECT

dotnet user-secrets set \
  "DeliveryAccounts:DeliveryOnePassword" \
  "$(get '.DeliveryAccounts.DeliveryOnePassword')" \
  --project $PROJECT

dotnet user-secrets set \
  "DeliveryAccounts:DeliveryOneFullName" \
  "$(get '.DeliveryAccounts.DeliveryOneFullName')" \
  --project $PROJECT

dotnet user-secrets set \
  "DeliveryAccounts:DeliveryOneEmail" \
  "$(get '.DeliveryAccounts.DeliveryOneEmail')" \
  --project $PROJECT

dotnet user-secrets set \
  "DeliveryAccounts:DeliveryOneJobTitle" \
  "$(get '.DeliveryAccounts.DeliveryOneJobTitle')" \
  --project $PROJECT

dotnet user-secrets set \
  "DeliveryAccounts:DeliveryOnePhoneNumber" \
  "$(get '.DeliveryAccounts.DeliveryOnePhoneNumber')" \
  --project $PROJECT

dotnet user-secrets set \
  "DeliveryAccounts:DeliveryOneConfiguration" \
  "$(get '.DeliveryAccounts.DeliveryOneConfiguration')" \
  --project $PROJECT

dotnet user-secrets set \
  "DeliveryAccounts:DeliveryOneIsEnabled" \
  "$(get '.DeliveryAccounts.DeliveryOneIsEnabled')" \
  --project $PROJECT

# 7) CUSTOMER ACCOUNTS REGISTER FLAG
dotnet user-secrets set \
  "CustomerAccountsRegister" \
  "$(get '.CustomerAccountsRegister')" \
  --project $PROJECT

# Customer One Account
dotnet user-secrets set \
  "CustomerAccounts:CustomerOneUserName" \
  "$(get '.CustomerAccounts.CustomerOneUserName')" \
  --project $PROJECT

dotnet user-secrets set \
  "CustomerAccounts:CustomerOnePassword" \
  "$(get '.CustomerAccounts.CustomerOnePassword')" \
  --project $PROJECT

dotnet user-secrets set \
  "CustomerAccounts:CustomerOneFullName" \
  "$(get '.CustomerAccounts.CustomerOneFullName')" \
  --project $PROJECT

dotnet user-secrets set \
  "CustomerAccounts:CustomerOneEmail" \
  "$(get '.CustomerAccounts.CustomerOneEmail')" \
  --project $PROJECT

dotnet user-secrets set \
  "CustomerAccounts:CustomerOneJobTitle" \
  "$(get '.CustomerAccounts.CustomerOneJobTitle')" \
  --project $PROJECT

dotnet user-secrets set \
  "CustomerAccounts:CustomerOnePhoneNumber" \
  "$(get '.CustomerAccounts.CustomerOnePhoneNumber')" \
  --project $PROJECT

dotnet user-secrets set \
  "CustomerAccounts:CustomerOneConfiguration" \
  "$(get '.CustomerAccounts.CustomerOneConfiguration')" \
  --project $PROJECT

dotnet user-secrets set \
  "CustomerAccounts:CustomerOneIsEnabled" \
  "$(get '.CustomerAccounts.CustomerOneIsEnabled')" \
  --project $PROJECT