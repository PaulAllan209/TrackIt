#!/bin/bash

# Initialize user secrets
dotnet user-secrets init --project TrackIt.Api.IntegrationTests

# Connection String
dotnet user-secrets set "ConnectionStrings:TrackIt" "${{ fromJSON(secrets.SERVER_SECRETS_JSON).ConnectionStrings.TrackIt }}" --project TrackIt.Server

# Admin Account
dotnet user-secrets set "DefaultAccounts:AdminUserName" "${{ fromJSON(secrets.SERVER_SECRETS_JSON).DefaultAccounts.AdminUserName }}" --project TrackIt.Server
dotnet user-secrets set "DefaultAccounts:AdminPassword" "${{ fromJSON(secrets.SERVER_SECRETS_JSON).DefaultAccounts.AdminPassword }}" --project TrackIt.Server
dotnet user-secrets set "DefaultAccounts:AdminEmail" "${{ fromJSON(secrets.SERVER_SECRETS_JSON).DefaultAccounts.AdminEmail }}" --project TrackIt.Server

# Supplier Accounts Registration Flag
dotnet user-secrets set "SupplierAccountsRegister" "${{ fromJSON(secrets.SERVER_SECRETS_JSON).SupplierAccountsRegister }}" --project TrackIt.Server

# Supplier Alpha Account
dotnet user-secrets set "SupplierAccounts:SupplierAlphaUserName" "${{ fromJSON(secrets.SERVER_SECRETS_JSON).SupplierAccounts.SupplierAlphaUserName }}" --project TrackIt.Server
dotnet user-secrets set "SupplierAccounts:SupplierAlphaPassword" "${{ fromJSON(secrets.SERVER_SECRETS_JSON).SupplierAccounts.SupplierAlphaPassword }}" --project TrackIt.Server
dotnet user-secrets set "SupplierAccounts:SupplierAlphaFullName" "${{ fromJSON(secrets.SERVER_SECRETS_JSON).SupplierAccounts.SupplierAlphaFullName }}" --project TrackIt.Server
dotnet user-secrets set "SupplierAccounts:SupplierAlphaEmail" "${{ fromJSON(secrets.SERVER_SECRETS_JSON).SupplierAccounts.SupplierAlphaEmail }}" --project TrackIt.Server
dotnet user-secrets set "SupplierAccounts:SupplierAlphaJobTitle" "${{ fromJSON(secrets.SERVER_SECRETS_JSON).SupplierAccounts.SupplierAlphaJobTitle }}" --project TrackIt.Server
dotnet user-secrets set "SupplierAccounts:SupplierAlphaPhoneNumber" "${{ fromJSON(secrets.SERVER_SECRETS_JSON).SupplierAccounts.SupplierAlphaPhoneNumber }}" --project TrackIt.Server
dotnet user-secrets set "SupplierAccounts:SupplierAlphaConfiguration" "${{ fromJSON(secrets.SERVER_SECRETS_JSON).SupplierAccounts.SupplierAlphaConfiguration }}" --project TrackIt.Server
dotnet user-secrets set "SupplierAccounts:SupplierAlphaIsEnabled" "${{ fromJSON(secrets.SERVER_SECRETS_JSON).SupplierAccounts.SupplierAlphaIsEnabled }}" --project TrackIt.Server

# Supplier Beta Account
dotnet user-secrets set "SupplierAccounts:SupplierBetaUserName" "${{ fromJSON(secrets.SERVER_SECRETS_JSON).SupplierAccounts.SupplierBetaUserName }}" --project TrackIt.Server
dotnet user-secrets set "SupplierAccounts:SupplierBetaPassword" "${{ fromJSON(secrets.SERVER_SECRETS_JSON).SupplierAccounts.SupplierBetaPassword }}" --project TrackIt.Server
dotnet user-secrets set "SupplierAccounts:SupplierBetaFullName" "${{ fromJSON(secrets.SERVER_SECRETS_JSON).SupplierAccounts.SupplierBetaFullName }}" --project TrackIt.Server
dotnet user-secrets set "SupplierAccounts:SupplierBetaEmail" "${{ fromJSON(secrets.SERVER_SECRETS_JSON).SupplierAccounts.SupplierBetaEmail }}" --project TrackIt.Server
dotnet user-secrets set "SupplierAccounts:SupplierBetaJobTitle" "${{ fromJSON(secrets.SERVER_SECRETS_JSON).SupplierAccounts.SupplierBetaJobTitle }}" --project TrackIt.Server
dotnet user-secrets set "SupplierAccounts:SupplierBetaPhoneNumber" "${{ fromJSON(secrets.SERVER_SECRETS_JSON).SupplierAccounts.SupplierBetaPhoneNumber }}" --project TrackIt.Server
dotnet user-secrets set "SupplierAccounts:SupplierBetaConfiguration" "${{ fromJSON(secrets.SERVER_SECRETS_JSON).SupplierAccounts.SupplierBetaConfiguration }}" --project TrackIt.Server
dotnet user-secrets set "SupplierAccounts:SupplierBetaIsEnabled" "${{ fromJSON(secrets.SERVER_SECRETS_JSON).SupplierAccounts.SupplierBetaIsEnabled }}" --project TrackIt.Server

# Supplier Gamma Account
dotnet user-secrets set "SupplierAccounts:SupplierGammaUserName" "${{ fromJSON(secrets.SERVER_SECRETS_JSON).SupplierAccounts.SupplierGammaUserName }}" --project TrackIt.Server
dotnet user-secrets set "SupplierAccounts:SupplierGammaPassword" "${{ fromJSON(secrets.SERVER_SECRETS_JSON).SupplierAccounts.SupplierGammaPassword }}" --project TrackIt.Server
dotnet user-secrets set "SupplierAccounts:SupplierGammaFullName" "${{ fromJSON(secrets.SERVER_SECRETS_JSON).SupplierAccounts.SupplierGammaFullName }}" --project TrackIt.Server
dotnet user-secrets set "SupplierAccounts:SupplierGammaEmail" "${{ fromJSON(secrets.SERVER_SECRETS_JSON).SupplierAccounts.SupplierGammaEmail }}" --project TrackIt.Server
dotnet user-secrets set "SupplierAccounts:SupplierGammaJobTitle" "${{ fromJSON(secrets.SERVER_SECRETS_JSON).SupplierAccounts.SupplierGammaJobTitle }}" --project TrackIt.Server
dotnet user-secrets set "SupplierAccounts:SupplierGammaPhoneNumber" "${{ fromJSON(secrets.SERVER_SECRETS_JSON).SupplierAccounts.SupplierGammaPhoneNumber }}" --project TrackIt.Server
dotnet user-secrets set "SupplierAccounts:SupplierGammaConfiguration" "${{ fromJSON(secrets.SERVER_SECRETS_JSON).SupplierAccounts.SupplierGammaConfiguration }}" --project TrackIt.Server
dotnet user-secrets set "SupplierAccounts:SupplierGammaIsEnabled" "${{ fromJSON(secrets.SERVER_SECRETS_JSON).SupplierAccounts.SupplierGammaIsEnabled }}" --project TrackIt.Server

# Facility Accounts Registration Flag
dotnet user-secrets set "FacilityAccountsRegister" "${{ fromJSON(secrets.SERVER_SECRETS_JSON).FacilityAccountsRegister }}" --project TrackIt.Server

# Facility One Account
dotnet user-secrets set "FacilityAccounts:FacilityOneUserName" "${{ fromJSON(secrets.SERVER_SECRETS_JSON).FacilityAccounts.FacilityOneUserName }}" --project TrackIt.Server
dotnet user-secrets set "FacilityAccounts:FacilityOnePassword" "${{ fromJSON(secrets.SERVER_SECRETS_JSON).FacilityAccounts.FacilityOnePassword }}" --project TrackIt.Server
dotnet user-secrets set "FacilityAccounts:FacilityOneFullName" "${{ fromJSON(secrets.SERVER_SECRETS_JSON).FacilityAccounts.FacilityOneFullName }}" --project TrackIt.Server
dotnet user-secrets set "FacilityAccounts:FacilityOneEmail" "${{ fromJSON(secrets.SERVER_SECRETS_JSON).FacilityAccounts.FacilityOneEmail }}" --project TrackIt.Server
dotnet user-secrets set "FacilityAccounts:FacilityOneJobTitle" "${{ fromJSON(secrets.SERVER_SECRETS_JSON).FacilityAccounts.FacilityOneJobTitle }}" --project TrackIt.Server
dotnet user-secrets set "FacilityAccounts:FacilityOnePhoneNumber" "${{ fromJSON(secrets.SERVER_SECRETS_JSON).FacilityAccounts.FacilityOnePhoneNumber }}" --project TrackIt.Server
dotnet user-secrets set "FacilityAccounts:FacilityOneConfiguration" "${{ fromJSON(secrets.SERVER_SECRETS_JSON).FacilityAccounts.FacilityOneConfiguration }}" --project TrackIt.Server
dotnet user-secrets set "FacilityAccounts:FacilityOneIsEnabled" "${{ fromJSON(secrets.SERVER_SECRETS_JSON).FacilityAccounts.FacilityOneIsEnabled }}" --project TrackIt.Server

# Delivery Accounts Registration Flag
dotnet user-secrets set "DeliveryAccountsRegister" "${{ fromJSON(secrets.SERVER_SECRETS_JSON).DeliveryAccountsRegister }}" --project TrackIt.Server

# Delivery One Account
dotnet user-secrets set "DeliveryAccounts:DeliveryOneUserName" "${{ fromJSON(secrets.SERVER_SECRETS_JSON).DeliveryAccounts.DeliveryOneUserName }}" --project TrackIt.Server
dotnet user-secrets set "DeliveryAccounts:DeliveryOnePassword" "${{ fromJSON(secrets.SERVER_SECRETS_JSON).DeliveryAccounts.DeliveryOnePassword }}" --project TrackIt.Server
dotnet user-secrets set "DeliveryAccounts:DeliveryOneFullName" "${{ fromJSON(secrets.SERVER_SECRETS_JSON).DeliveryAccounts.DeliveryOneFullName }}" --project TrackIt.Server
dotnet user-secrets set "DeliveryAccounts:DeliveryOneEmail" "${{ fromJSON(secrets.SERVER_SECRETS_JSON).DeliveryAccounts.DeliveryOneEmail }}" --project TrackIt.Server
dotnet user-secrets set "DeliveryAccounts:DeliveryOneJobTitle" "${{ fromJSON(secrets.SERVER_SECRETS_JSON).DeliveryAccounts.DeliveryOneJobTitle }}" --project TrackIt.Server
dotnet user-secrets set "DeliveryAccounts:DeliveryOnePhoneNumber" "${{ fromJSON(secrets.SERVER_SECRETS_JSON).DeliveryAccounts.DeliveryOnePhoneNumber }}" --project TrackIt.Server
dotnet user-secrets set "DeliveryAccounts:DeliveryOneConfiguration" "${{ fromJSON(secrets.SERVER_SECRETS_JSON).DeliveryAccounts.DeliveryOneConfiguration }}" --project TrackIt.Server
dotnet user-secrets set "DeliveryAccounts:DeliveryOneIsEnabled" "${{ fromJSON(secrets.SERVER_SECRETS_JSON).DeliveryAccounts.DeliveryOneIsEnabled }}" --project TrackIt.Server

# Customer Accounts Registration Flag
dotnet user-secrets set "CustomerAccountsRegister" "${{ fromJSON(secrets.SERVER_SECRETS_JSON).CustomerAccountsRegister }}" --project TrackIt.Server

# Customer One Account
dotnet user-secrets set "CustomerAccounts:CustomerOneUserName" "${{ fromJSON(secrets.SERVER_SECRETS_JSON).CustomerAccounts.CustomerOneUserName }}" --project TrackIt.Server
dotnet user-secrets set "CustomerAccounts:CustomerOnePassword" "${{ fromJSON(secrets.SERVER_SECRETS_JSON).CustomerAccounts.CustomerOnePassword }}" --project TrackIt.Server
dotnet user-secrets set "CustomerAccounts:CustomerOneFullName" "${{ fromJSON(secrets.SERVER_SECRETS_JSON).CustomerAccounts.CustomerOneFullName }}" --project TrackIt.Server
dotnet user-secrets set "CustomerAccounts:CustomerOneEmail" "${{ fromJSON(secrets.SERVER_SECRETS_JSON).CustomerAccounts.CustomerOneEmail }}" --project TrackIt.Server
dotnet user-secrets set "CustomerAccounts:CustomerOneJobTitle" "${{ fromJSON(secrets.SERVER_SECRETS_JSON).CustomerAccounts.CustomerOneJobTitle }}" --project TrackIt.Server
dotnet user-secrets set "CustomerAccounts:CustomerOnePhoneNumber" "${{ fromJSON(secrets.SERVER_SECRETS_JSON).CustomerAccounts.CustomerOnePhoneNumber }}" --project TrackIt.Server
dotnet user-secrets set "CustomerAccounts:CustomerOneConfiguration" "${{ fromJSON(secrets.SERVER_SECRETS_JSON).CustomerAccounts.CustomerOneConfiguration }}" --project TrackIt.Server
dotnet user-secrets set "CustomerAccounts:CustomerOneIsEnabled" "${{ fromJSON(secrets.SERVER_SECRETS_JSON).CustomerAccounts.CustomerOneIsEnabled }}" --project TrackIt.Server