#!/bin/bash

# Initialize user secrets for the project
dotnet user-secrets init --project TrackIt.Server

# Kestrel Certificate
dotnet user-secrets set "Kestrel:Certificates:Development:Password" "${{ fromJSON(secrets.SERVER_SECRETS_JSON)['Kestrel:Certificates:Development:Password'] }}" --project TrackIt.Server

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

# Supplier Delta Account
dotnet user-secrets set "SupplierAccounts:SupplierDeltaUserName" "${{ fromJSON(secrets.SERVER_SECRETS_JSON).SupplierAccounts.SupplierDeltaUserName }}" --project TrackIt.Server
dotnet user-secrets set "SupplierAccounts:SupplierDeltaPassword" "${{ fromJSON(secrets.SERVER_SECRETS_JSON).SupplierAccounts.SupplierDeltaPassword }}" --project TrackIt.Server
dotnet user-secrets set "SupplierAccounts:SupplierDeltaFullName" "${{ fromJSON(secrets.SERVER_SECRETS_JSON).SupplierAccounts.SupplierDeltaFullName }}" --project TrackIt.Server
dotnet user-secrets set "SupplierAccounts:SupplierDeltaEmail" "${{ fromJSON(secrets.SERVER_SECRETS_JSON).SupplierAccounts.SupplierDeltaEmail }}" --project TrackIt.Server
dotnet user-secrets set "SupplierAccounts:SupplierDeltaJobTitle" "${{ fromJSON(secrets.SERVER_SECRETS_JSON).SupplierAccounts.SupplierDeltaJobTitle }}" --project TrackIt.Server
dotnet user-secrets set "SupplierAccounts:SupplierDeltaPhoneNumber" "${{ fromJSON(secrets.SERVER_SECRETS_JSON).SupplierAccounts.SupplierDeltaPhoneNumber }}" --project TrackIt.Server
dotnet user-secrets set "SupplierAccounts:SupplierDeltaConfiguration" "${{ fromJSON(secrets.SERVER_SECRETS_JSON).SupplierAccounts.SupplierDeltaConfiguration }}" --project TrackIt.Server
dotnet user-secrets set "SupplierAccounts:SupplierDeltaIsEnabled" "${{ fromJSON(secrets.SERVER_SECRETS_JSON).SupplierAccounts.SupplierDeltaIsEnabled }}" --project TrackIt.Server

# Supplier Epsilon Account
dotnet user-secrets set "SupplierAccounts:SupplierEpsilonUserName" "${{ fromJSON(secrets.SERVER_SECRETS_JSON).SupplierAccounts.SupplierEpsilonUserName }}" --project TrackIt.Server
dotnet user-secrets set "SupplierAccounts:SupplierEpsilonPassword" "${{ fromJSON(secrets.SERVER_SECRETS_JSON).SupplierAccounts.SupplierEpsilonPassword }}" --project TrackIt.Server
dotnet user-secrets set "SupplierAccounts:SupplierEpsilonFullName" "${{ fromJSON(secrets.SERVER_SECRETS_JSON).SupplierAccounts.SupplierEpsilonFullName }}" --project TrackIt.Server
dotnet user-secrets set "SupplierAccounts:SupplierEpsilonEmail" "${{ fromJSON(secrets.SERVER_SECRETS_JSON).SupplierAccounts.SupplierEpsilonEmail }}" --project TrackIt.Server
dotnet user-secrets set "SupplierAccounts:SupplierEpsilonJobTitle" "${{ fromJSON(secrets.SERVER_SECRETS_JSON).SupplierAccounts.SupplierEpsilonJobTitle }}" --project TrackIt.Server
dotnet user-secrets set "SupplierAccounts:SupplierEpsilonPhoneNumber" "${{ fromJSON(secrets.SERVER_SECRETS_JSON).SupplierAccounts.SupplierEpsilonPhoneNumber }}" --project TrackIt.Server
dotnet user-secrets set "SupplierAccounts:SupplierEpsilonConfiguration" "${{ fromJSON(secrets.SERVER_SECRETS_JSON).SupplierAccounts.SupplierEpsilonConfiguration }}" --project TrackIt.Server
dotnet user-secrets set "SupplierAccounts:SupplierEpsilonIsEnabled" "${{ fromJSON(secrets.SERVER_SECRETS_JSON).SupplierAccounts.SupplierEpsilonIsEnabled }}" --project TrackIt.Server

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

# Facility Two Account
dotnet user-secrets set "FacilityAccounts:FacilityTwoUserName" "${{ fromJSON(secrets.SERVER_SECRETS_JSON).FacilityAccounts.FacilityTwoUserName }}" --project TrackIt.Server
dotnet user-secrets set "FacilityAccounts:FacilityTwoPassword" "${{ fromJSON(secrets.SERVER_SECRETS_JSON).FacilityAccounts.FacilityTwoPassword }}" --project TrackIt.Server
dotnet user-secrets set "FacilityAccounts:FacilityTwoFullName" "${{ fromJSON(secrets.SERVER_SECRETS_JSON).FacilityAccounts.FacilityTwoFullName }}" --project TrackIt.Server
dotnet user-secrets set "FacilityAccounts:FacilityTwoEmail" "${{ fromJSON(secrets.SERVER_SECRETS_JSON).FacilityAccounts.FacilityTwoEmail }}" --project TrackIt.Server
dotnet user-secrets set "FacilityAccounts:FacilityTwoJobTitle" "${{ fromJSON(secrets.SERVER_SECRETS_JSON).FacilityAccounts.FacilityTwoJobTitle }}" --project TrackIt.Server
dotnet user-secrets set "FacilityAccounts:FacilityTwoPhoneNumber" "${{ fromJSON(secrets.SERVER_SECRETS_JSON).FacilityAccounts.FacilityTwoPhoneNumber }}" --project TrackIt.Server
dotnet user-secrets set "FacilityAccounts:FacilityTwoConfiguration" "${{ fromJSON(secrets.SERVER_SECRETS_JSON).FacilityAccounts.FacilityTwoConfiguration }}" --project TrackIt.Server
dotnet user-secrets set "FacilityAccounts:FacilityTwoIsEnabled" "${{ fromJSON(secrets.SERVER_SECRETS_JSON).FacilityAccounts.FacilityTwoIsEnabled }}" --project TrackIt.Server

# Facility Three Account
dotnet user-secrets set "FacilityAccounts:FacilityThreeUserName" "${{ fromJSON(secrets.SERVER_SECRETS_JSON).FacilityAccounts.FacilityThreeUserName }}" --project TrackIt.Server
dotnet user-secrets set "FacilityAccounts:FacilityThreePassword" "${{ fromJSON(secrets.SERVER_SECRETS_JSON).FacilityAccounts.FacilityThreePassword }}" --project TrackIt.Server
dotnet user-secrets set "FacilityAccounts:FacilityThreeFullName" "${{ fromJSON(secrets.SERVER_SECRETS_JSON).FacilityAccounts.FacilityThreeFullName }}" --project TrackIt.Server
dotnet user-secrets set "FacilityAccounts:FacilityThreeEmail" "${{ fromJSON(secrets.SERVER_SECRETS_JSON).FacilityAccounts.FacilityThreeEmail }}" --project TrackIt.Server
dotnet user-secrets set "FacilityAccounts:FacilityThreeJobTitle" "${{ fromJSON(secrets.SERVER_SECRETS_JSON).FacilityAccounts.FacilityThreeJobTitle }}" --project TrackIt.Server
dotnet user-secrets set "FacilityAccounts:FacilityThreePhoneNumber" "${{ fromJSON(secrets.SERVER_SECRETS_JSON).FacilityAccounts.FacilityThreePhoneNumber }}" --project TrackIt.Server
dotnet user-secrets set "FacilityAccounts:FacilityThreeConfiguration" "${{ fromJSON(secrets.SERVER_SECRETS_JSON).FacilityAccounts.FacilityThreeConfiguration }}" --project TrackIt.Server
dotnet user-secrets set "FacilityAccounts:FacilityThreeIsEnabled" "${{ fromJSON(secrets.SERVER_SECRETS_JSON).FacilityAccounts.FacilityThreeIsEnabled }}" --project TrackIt.Server

# Facility Four Account
dotnet user-secrets set "FacilityAccounts:FacilityFourUserName" "${{ fromJSON(secrets.SERVER_SECRETS_JSON).FacilityAccounts.FacilityFourUserName }}" --project TrackIt.Server
dotnet user-secrets set "FacilityAccounts:FacilityFourPassword" "${{ fromJSON(secrets.SERVER_SECRETS_JSON).FacilityAccounts.FacilityFourPassword }}" --project TrackIt.Server
dotnet user-secrets set "FacilityAccounts:FacilityFourFullName" "${{ fromJSON(secrets.SERVER_SECRETS_JSON).FacilityAccounts.FacilityFourFullName }}" --project TrackIt.Server
dotnet user-secrets set "FacilityAccounts:FacilityFourEmail" "${{ fromJSON(secrets.SERVER_SECRETS_JSON).FacilityAccounts.FacilityFourEmail }}" --project TrackIt.Server
dotnet user-secrets set "FacilityAccounts:FacilityFourJobTitle" "${{ fromJSON(secrets.SERVER_SECRETS_JSON).FacilityAccounts.FacilityFourJobTitle }}" --project TrackIt.Server
dotnet user-secrets set "FacilityAccounts:FacilityFourPhoneNumber" "${{ fromJSON(secrets.SERVER_SECRETS_JSON).FacilityAccounts.FacilityFourPhoneNumber }}" --project TrackIt.Server
dotnet user-secrets set "FacilityAccounts:FacilityFourConfiguration" "${{ fromJSON(secrets.SERVER_SECRETS_JSON).FacilityAccounts.FacilityFourConfiguration }}" --project TrackIt.Server
dotnet user-secrets set "FacilityAccounts:FacilityFourIsEnabled" "${{ fromJSON(secrets.SERVER_SECRETS_JSON).FacilityAccounts.FacilityFourIsEnabled }}" --project TrackIt.Server

# Facility Five Account
dotnet user-secrets set "FacilityAccounts:FacilityFiveUserName" "${{ fromJSON(secrets.SERVER_SECRETS_JSON).FacilityAccounts.FacilityFiveUserName }}" --project TrackIt.Server
dotnet user-secrets set "FacilityAccounts:FacilityFivePassword" "${{ fromJSON(secrets.SERVER_SECRETS_JSON).FacilityAccounts.FacilityFivePassword }}" --project TrackIt.Server
dotnet user-secrets set "FacilityAccounts:FacilityFiveFullName" "${{ fromJSON(secrets.SERVER_SECRETS_JSON).FacilityAccounts.FacilityFiveFullName }}" --project TrackIt.Server
dotnet user-secrets set "FacilityAccounts:FacilityFiveEmail" "${{ fromJSON(secrets.SERVER_SECRETS_JSON).FacilityAccounts.FacilityFiveEmail }}" --project TrackIt.Server
dotnet user-secrets set "FacilityAccounts:FacilityFiveJobTitle" "${{ fromJSON(secrets.SERVER_SECRETS_JSON).FacilityAccounts.FacilityFiveJobTitle }}" --project TrackIt.Server
dotnet user-secrets set "FacilityAccounts:FacilityFivePhoneNumber" "${{ fromJSON(secrets.SERVER_SECRETS_JSON).FacilityAccounts.FacilityFivePhoneNumber }}" --project TrackIt.Server
dotnet user-secrets set "FacilityAccounts:FacilityFiveConfiguration" "${{ fromJSON(secrets.SERVER_SECRETS_JSON).FacilityAccounts.FacilityFiveConfiguration }}" --project TrackIt.Server
dotnet user-secrets set "FacilityAccounts:FacilityFiveIsEnabled" "${{ fromJSON(secrets.SERVER_SECRETS_JSON).FacilityAccounts.FacilityFiveIsEnabled }}" --project TrackIt.Server

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