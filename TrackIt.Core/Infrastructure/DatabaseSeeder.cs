using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using TrackIt.Core.Interfaces;
using TrackIt.Core.Models;
using TrackIt.Core.Models.Account;
using TrackIt.Core.Services.Account;

namespace TrackIt.Core.Infrastructure
{
    public class DatabaseSeeder : IDatabaseSeeder
    {
        private readonly ApplicationDbContext dbContext;
        private readonly ILogger<DatabaseSeeder> logger;
        private readonly IUserAccountService userAccountService;
        private readonly IUserRoleService userRoleService;
        private readonly IConfiguration configuration;

        public DatabaseSeeder(
            ApplicationDbContext dbContext, 
            ILogger<DatabaseSeeder> logger,
            IUserAccountService userAccountService, 
            IUserRoleService userRoleService,
            IConfiguration configuration)
        {
            this.dbContext = dbContext;
            this.logger = logger;
            this.userAccountService = userAccountService;
            this.userRoleService = userRoleService;
            this.configuration = configuration;
        }
        public async Task SeedAsync()
        {
            await dbContext.Database.MigrateAsync();
            await SeedDefaultUsersAsync();
        }

        /************ DEFAULT USERS **************/

        private async Task SeedDefaultUsersAsync()
        {
            if (!await dbContext.Users.AnyAsync())
            {
                logger.LogInformation("Generating inbuilt accounts");

                const string adminRoleName = "administrator";
                const string userRoleName = "user";
                const string supplierRoleName = "supplier";
                const string facilityRoleName = "facility";
                const string deliveryRoleName = "delivery";
                const string customerRoleName = "customer";

                await EnsureRoleAsync(adminRoleName, "Default administrator",
                    ApplicationPermissions.GetAllPermissionValues());

                await EnsureRoleAsync(userRoleName, "Default user", Array.Empty<string>());

                await EnsureRoleAsync(supplierRoleName, "The supplier role", 
                new[]
                {
                    ApplicationPermissions.CreateShipment.Value,
                    ApplicationPermissions.ViewShipment.Value,
                    ApplicationPermissions.UpdateShipment.Value,
                    ApplicationPermissions.DeleteShipment.Value,
                    ApplicationPermissions.ViewStatusHistory.Value,
                    ApplicationPermissions.CreateStatus.Value,
                    ApplicationPermissions.UpdateStatus.Value,

                    ApplicationPermissions.ViewUsers
                });

                await EnsureRoleAsync(facilityRoleName, "The facility role", 
                new[]
                {
                    ApplicationPermissions.ViewShipment.Value,
                    ApplicationPermissions.CreateStatus.Value,
                    ApplicationPermissions.UpdateStatus.Value,
                    ApplicationPermissions.ViewStatusHistory.Value,

                    ApplicationPermissions.ViewUsers
                });

                await EnsureRoleAsync(deliveryRoleName, "The delivery role", 
                new[]
                {
                    ApplicationPermissions.ViewShipment.Value,
                    ApplicationPermissions.CreateStatus.Value,
                    ApplicationPermissions.UpdateStatus.Value,
                    ApplicationPermissions.ViewStatusHistory.Value,

                    ApplicationPermissions.ViewUsers
                });

                await EnsureRoleAsync(customerRoleName, "The customer role",
                new[]
                {
                    ApplicationPermissions.ViewShipment.Value,
                    ApplicationPermissions.CreateStatus.Value,
                    ApplicationPermissions.ViewStatusHistory.Value,
                    ApplicationPermissions.SetStatusDelivered.Value,

                    ApplicationPermissions.ViewUsers
                });

                // Get values from user secrets
                var adminUserName = configuration["DefaultAccounts:AdminUserName"] ?? "admin";
                var adminPassword = configuration["DefaultAccounts:AdminPassword"] ?? "Password123";
                var adminEmail = configuration["DefaultAccounts:AdminEmail"] ?? "admin@gmail.com";

                await CreateUserAsync(adminUserName,
                                      adminPassword,
                                      "Inbuilt Administrator",
                                      adminEmail,
                                      "+1 (123) 000-0000",
                                      new[] { adminRoleName });

                /*********************** SUPPLIER ACCOUNTS ***********************/
                var supplierAlphaUserName = configuration["SupplierAccounts:SupplierAlphaUserName"];
                var supplierAlphaPassword = configuration["SupplierAccounts:SupplierAlphaPassword"];
                var supplierAlphaFullName = configuration["SupplierAccounts:SupplierAlphaFullName"];
                var supplierAlphaEmail = configuration["SupplierAccounts:SupplierAlphaEmail"];
                var supplierAlphaPhoneNumber = configuration["SupplierAccounts:SupplierAlphaPhoneNumber"];

                var supplierBetaUserName = configuration["SupplierAccounts:SupplierBetaUserName"];
                var supplierBetaPassword = configuration["SupplierAccounts:SupplierBetaPassword"];
                var supplierBetaFullName = configuration["SupplierAccounts:SupplierBetaFullName"];
                var supplierBetaEmail = configuration["SupplierAccounts:SupplierBetaEmail"];
                var supplierBetaPhoneNumber = configuration["SupplierAccounts:SupplierBetaPhoneNumber"];

                var supplierGammaUserName = configuration["SupplierAccounts:SupplierGammaUserName"];
                var supplierGammaPassword = configuration["SupplierAccounts:SupplierGammaPassword"];
                var supplierGammaFullName = configuration["SupplierAccounts:SupplierGammaFullName"];
                var supplierGammaEmail = configuration["SupplierAccounts:SupplierGammaEmail"];
                var supplierGammaPhoneNumber = configuration["SupplierAccounts:SupplierGammaPhoneNumber"];

                var supplierDeltaUserName = configuration["SupplierAccounts:SupplierDeltaUserName"];
                var supplierDeltaPassword = configuration["SupplierAccounts:SupplierDeltaPassword"];
                var supplierDeltaFullName = configuration["SupplierAccounts:SupplierDeltaFullName"];
                var supplierDeltaEmail = configuration["SupplierAccounts:SupplierDeltaEmail"];
                var supplierDeltaPhoneNumber = configuration["SupplierAccounts:SupplierDeltaPhoneNumber"];

                var supplierEpsilonUserName = configuration["SupplierAccounts:SupplierEpsilonUserName"];
                var supplierEpsilonPassword = configuration["SupplierAccounts:SupplierEpsilonPassword"];
                var supplierEpsilonFullName = configuration["SupplierAccounts:SupplierEpsilonFullName"];
                var supplierEpsilonEmail = configuration["SupplierAccounts:SupplierEpsilonEmail"];
                var supplierEpsilonPhoneNumber = configuration["SupplierAccounts:SupplierEpsilonPhoneNumber"];

                var registerSuppliers = configuration.GetValue<bool>("SupplierAccountsRegister");
                if (registerSuppliers)
                {
                    if (supplierAlphaUserName != null && supplierAlphaPassword != null && supplierAlphaFullName != null && supplierAlphaEmail != null && supplierAlphaPhoneNumber != null)
                    {
                        await CreateUserAsync(supplierAlphaUserName,
                                              supplierAlphaPassword,
                                              supplierAlphaFullName,
                                              supplierAlphaEmail,
                                              supplierAlphaPhoneNumber,
                                              new[] { supplierRoleName });
                    }
                    if (supplierBetaUserName != null && supplierBetaPassword != null && supplierBetaFullName != null && supplierBetaEmail != null && supplierBetaPhoneNumber != null)
                    {
                        await CreateUserAsync(supplierBetaUserName,
                                              supplierBetaPassword,
                                              supplierBetaFullName,
                                              supplierBetaEmail,
                                              supplierBetaPhoneNumber,
                                              new[] { supplierRoleName });
                    }
                    if (supplierGammaUserName != null && supplierGammaPassword != null && supplierGammaFullName != null && supplierGammaEmail != null && supplierGammaPhoneNumber != null)
                    {
                        await CreateUserAsync(supplierGammaUserName,
                                              supplierGammaPassword,
                                              supplierGammaFullName,
                                              supplierGammaEmail,
                                              supplierGammaPhoneNumber,
                                              new[] { supplierRoleName });
                    }
                    if (supplierDeltaUserName != null && supplierDeltaPassword != null && supplierDeltaFullName != null && supplierDeltaEmail != null && supplierDeltaPhoneNumber != null)
                    {
                        await CreateUserAsync(supplierDeltaUserName,
                                              supplierDeltaPassword,
                                              supplierDeltaFullName,
                                              supplierDeltaEmail,
                                              supplierDeltaPhoneNumber,
                                              new[] { supplierRoleName });
                    }
                    if (supplierEpsilonUserName != null && supplierEpsilonPassword != null && supplierEpsilonFullName != null && supplierEpsilonEmail != null && supplierEpsilonPhoneNumber != null)
                    {
                        await CreateUserAsync(supplierEpsilonUserName,
                                              supplierEpsilonPassword,
                                              supplierEpsilonFullName,
                                              supplierEpsilonEmail,
                                              supplierEpsilonPhoneNumber,
                                              new[] { supplierRoleName });
                    }
                }


                /*********************** FACILITY ACCOUNTS ***********************/
                var facilityOneUserName = configuration["FacilityAccounts:FacilityOneUserName"];
                var facilityOnePassword = configuration["FacilityAccounts:FacilityOnePassword"];
                var facilityOneFullName = configuration["FacilityAccounts:FacilityOneFullName"];
                var facilityOneEmail = configuration["FacilityAccounts:FacilityOneEmail"];
                var facilityOnePhoneNumber = configuration["FacilityAccounts:FacilityOnePhoneNumber"];

                var facilityTwoUserName = configuration["FacilityAccounts:FacilityTwoUserName"];
                var facilityTwoPassword = configuration["FacilityAccounts:FacilityTwoPassword"];
                var facilityTwoFullName = configuration["FacilityAccounts:FacilityTwoFullName"];
                var facilityTwoEmail = configuration["FacilityAccounts:FacilityTwoEmail"];
                var facilityTwoPhoneNumber = configuration["FacilityAccounts:FacilityTwoPhoneNumber"];

                var facilityThreeUserName = configuration["FacilityAccounts:FacilityThreeUserName"];
                var facilityThreePassword = configuration["FacilityAccounts:FacilityThreePassword"];
                var facilityThreeFullName = configuration["FacilityAccounts:FacilityThreeFullName"];
                var facilityThreeEmail = configuration["FacilityAccounts:FacilityThreeEmail"];
                var facilityThreePhoneNumber = configuration["FacilityAccounts:FacilityThreePhoneNumber"];

                var facilityFourUserName = configuration["FacilityAccounts:FacilityFourUserName"];
                var facilityFourPassword = configuration["FacilityAccounts:FacilityFourPassword"];
                var facilityFourFullName = configuration["FacilityAccounts:FacilityFourFullName"];
                var facilityFourEmail = configuration["FacilityAccounts:FacilityFourEmail"];
                var facilityFourPhoneNumber = configuration["FacilityAccounts:FacilityFourPhoneNumber"];

                var facilityFiveUserName = configuration["FacilityAccounts:FacilityFiveUserName"];
                var facilityFivePassword = configuration["FacilityAccounts:FacilityFivePassword"];
                var facilityFiveFullName = configuration["FacilityAccounts:FacilityFiveFullName"];
                var facilityFiveEmail = configuration["FacilityAccounts:FacilityFiveEmail"];
                var facilityFivePhoneNumber = configuration["FacilityAccounts:FacilityFivePhoneNumber"];

                var registerFacilities = configuration.GetValue<bool>("FacilityAccountsRegister");
                if (registerFacilities)
                {
                    if (facilityOneUserName != null && facilityOnePassword != null && facilityOneFullName != null && facilityOneEmail != null && facilityOnePhoneNumber != null)
                    {
                        await CreateUserAsync(facilityOneUserName,
                                              facilityOnePassword,
                                              facilityOneFullName,
                                              facilityOneEmail,
                                              facilityOnePhoneNumber,
                                              new[] { facilityRoleName });
                    }
                    if (facilityTwoUserName != null && facilityTwoPassword != null && facilityTwoFullName != null && facilityTwoEmail != null && facilityTwoPhoneNumber != null)
                    {
                        await CreateUserAsync(facilityTwoUserName,
                                              facilityTwoPassword,
                                              facilityTwoFullName,
                                              facilityTwoEmail,
                                              facilityTwoPhoneNumber,
                                              new[] { facilityRoleName });
                    }
                    if (facilityThreeUserName != null && facilityThreePassword != null && facilityThreeFullName != null && facilityThreeEmail != null && facilityThreePhoneNumber != null)
                    {
                        await CreateUserAsync(facilityThreeUserName,
                                              facilityThreePassword,
                                              facilityThreeFullName,
                                              facilityThreeEmail,
                                              facilityThreePhoneNumber,
                                              new[] { facilityRoleName });
                    }
                    if (facilityFourUserName != null && facilityFourPassword != null && facilityFourFullName != null && facilityFourEmail != null && facilityFourPhoneNumber != null)
                    {
                        await CreateUserAsync(facilityFourUserName,
                                              facilityFourPassword,
                                              facilityFourFullName,
                                              facilityFourEmail,
                                              facilityFourPhoneNumber,
                                              new[] { facilityRoleName });
                    }
                    if (facilityFiveUserName != null && facilityFivePassword != null && facilityFiveFullName != null && facilityFiveEmail != null && facilityFivePhoneNumber != null)
                    {
                        await CreateUserAsync(facilityFiveUserName,
                                              facilityFivePassword,
                                              facilityFiveFullName,
                                              facilityFiveEmail,
                                              facilityFivePhoneNumber,
                                              new[] { facilityRoleName });
                    }
                }

                /*********************** DELIVERY ACCOUNTS ***********************/
                var deliveryOneUserName = configuration["DeliveryAccounts:DeliveryOneUserName"];
                var deliveryOnePassword = configuration["DeliveryAccounts:DeliveryOnePassword"];
                var deliveryOneFullName = configuration["DeliveryAccounts:DeliveryOneFullName"];
                var deliveryOneEmail = configuration["DeliveryAccounts:DeliveryOneEmail"];
                var deliveryOnePhoneNumber = configuration["DeliveryAccounts:DeliveryOnePhoneNumber"];

                var deliveryTwoUserName = configuration["DeliveryAccounts:DeliveryTwoUserName"];
                var deliveryTwoPassword = configuration["DeliveryAccounts:DeliveryTwoPassword"];
                var deliveryTwoFullName = configuration["DeliveryAccounts:DeliveryTwoFullName"];
                var deliveryTwoEmail = configuration["DeliveryAccounts:DeliveryTwoEmail"];
                var deliveryTwoPhoneNumber = configuration["DeliveryAccounts:DeliveryTwoPhoneNumber"];

                var deliveryThreeUserName = configuration["DeliveryAccounts:DeliveryThreeUserName"];
                var deliveryThreePassword = configuration["DeliveryAccounts:DeliveryThreePassword"];
                var deliveryThreeFullName = configuration["DeliveryAccounts:DeliveryThreeFullName"];
                var deliveryThreeEmail = configuration["DeliveryAccounts:DeliveryThreeEmail"];
                var deliveryThreePhoneNumber = configuration["DeliveryAccounts:DeliveryThreePhoneNumber"];

                var deliveryFourUserName = configuration["DeliveryAccounts:DeliveryFourUserName"];
                var deliveryFourPassword = configuration["DeliveryAccounts:DeliveryFourPassword"];
                var deliveryFourFullName = configuration["DeliveryAccounts:DeliveryFourFullName"];
                var deliveryFourEmail = configuration["DeliveryAccounts:DeliveryFourEmail"];
                var deliveryFourPhoneNumber = configuration["DeliveryAccounts:DeliveryFourPhoneNumber"];

                var deliveryFiveUserName = configuration["DeliveryAccounts:DeliveryFiveUserName"];
                var deliveryFivePassword = configuration["DeliveryAccounts:DeliveryFivePassword"];
                var deliveryFiveFullName = configuration["DeliveryAccounts:DeliveryFiveFullName"];
                var deliveryFiveEmail = configuration["DeliveryAccounts:DeliveryFiveEmail"];
                var deliveryFivePhoneNumber = configuration["DeliveryAccounts:DeliveryFivePhoneNumber"];

                var registerDeliveries = configuration.GetValue<bool>("DeliveryAccountsRegister");
                if (registerDeliveries)
                {
                    if (deliveryOneUserName != null && deliveryOnePassword != null && deliveryOneFullName != null && deliveryOneEmail != null && deliveryOnePhoneNumber != null)
                    {
                        await CreateUserAsync(deliveryOneUserName,
                                              deliveryOnePassword,
                                              deliveryOneFullName,
                                              deliveryOneEmail,
                                              deliveryOnePhoneNumber,
                                              new[] { deliveryRoleName });
                    }
                    if (deliveryTwoUserName != null && deliveryTwoPassword != null && deliveryTwoFullName != null && deliveryTwoEmail != null && deliveryTwoPhoneNumber != null)
                    {
                        await CreateUserAsync(deliveryTwoUserName,
                                              deliveryTwoPassword,
                                              deliveryTwoFullName,
                                              deliveryTwoEmail,
                                              deliveryTwoPhoneNumber,
                                              new[] { deliveryRoleName });
                    }
                    if (deliveryThreeUserName != null && deliveryThreePassword != null && deliveryThreeFullName != null && deliveryThreeEmail != null && deliveryThreePhoneNumber != null)
                    {
                        await CreateUserAsync(deliveryThreeUserName,
                                              deliveryThreePassword,
                                              deliveryThreeFullName,
                                              deliveryThreeEmail,
                                              deliveryThreePhoneNumber,
                                              new[] { deliveryRoleName });
                    }
                    if (deliveryFourUserName != null && deliveryFourPassword != null && deliveryFourFullName != null && deliveryFourEmail != null && deliveryFourPhoneNumber != null)
                    {
                        await CreateUserAsync(deliveryFourUserName,
                                              deliveryFourPassword,
                                              deliveryFourFullName,
                                              deliveryFourEmail,
                                              deliveryFourPhoneNumber,
                                              new[] { deliveryRoleName });
                    }
                    if (deliveryFiveUserName != null && deliveryFivePassword != null && deliveryFiveFullName != null && deliveryFiveEmail != null && deliveryFivePhoneNumber != null)
                    {
                        await CreateUserAsync(deliveryFiveUserName,
                                              deliveryFivePassword,
                                              deliveryFiveFullName,
                                              deliveryFiveEmail,
                                              deliveryFivePhoneNumber,
                                              new[] { deliveryRoleName });
                    }
                }

                /*********************** CUSTOMER ACCOUNTS ***********************/
                var customerOneUserName = configuration["CustomerAccounts:CustomerOneUserName"];
                var customerOnePassword = configuration["CustomerAccounts:CustomerOnePassword"];
                var customerOneFullName = configuration["CustomerAccounts:CustomerOneFullName"];
                var customerOneEmail = configuration["CustomerAccounts:CustomerOneEmail"];
                var customerOnePhoneNumber = configuration["CustomerAccounts:CustomerOnePhoneNumber"];

                var customerTwoUserName = configuration["CustomerAccounts:CustomerTwoUserName"];
                var customerTwoPassword = configuration["CustomerAccounts:CustomerTwoPassword"];
                var customerTwoFullName = configuration["CustomerAccounts:CustomerTwoFullName"];
                var customerTwoEmail = configuration["CustomerAccounts:CustomerTwoEmail"];
                var customerTwoPhoneNumber = configuration["CustomerAccounts:CustomerTwoPhoneNumber"];

                var customerThreeUserName = configuration["CustomerAccounts:CustomerThreeUserName"];
                var customerThreePassword = configuration["CustomerAccounts:CustomerThreePassword"];
                var customerThreeFullName = configuration["CustomerAccounts:CustomerThreeFullName"];
                var customerThreeEmail = configuration["CustomerAccounts:CustomerThreeEmail"];
                var customerThreePhoneNumber = configuration["CustomerAccounts:CustomerThreePhoneNumber"];

                var customerFourUserName = configuration["CustomerAccounts:CustomerFourUserName"];
                var customerFourPassword = configuration["CustomerAccounts:CustomerFourPassword"];
                var customerFourFullName = configuration["CustomerAccounts:CustomerFourFullName"];
                var customerFourEmail = configuration["CustomerAccounts:CustomerFourEmail"];
                var customerFourPhoneNumber = configuration["CustomerAccounts:CustomerFourPhoneNumber"];

                var customerFiveUserName = configuration["CustomerAccounts:CustomerFiveUserName"];
                var customerFivePassword = configuration["CustomerAccounts:CustomerFivePassword"];
                var customerFiveFullName = configuration["CustomerAccounts:CustomerFiveFullName"];
                var customerFiveEmail = configuration["CustomerAccounts:CustomerFiveEmail"];
                var customerFivePhoneNumber = configuration["CustomerAccounts:CustomerFivePhoneNumber"];

                var registerCustomers = configuration.GetValue<bool>("CustomerAccountsRegister");
                if (registerCustomers)
                {
                    if (customerOneUserName != null && customerOnePassword != null && customerOneFullName != null && customerOneEmail != null && customerOnePhoneNumber != null)
                    {
                        await CreateUserAsync(customerOneUserName,
                                              customerOnePassword,
                                              customerOneFullName,
                                              customerOneEmail,
                                              customerOnePhoneNumber,
                                              new[] { customerRoleName });
                    }
                    if (customerTwoUserName != null && customerTwoPassword != null && customerTwoFullName != null && customerTwoEmail != null && customerTwoPhoneNumber != null)
                    {
                        await CreateUserAsync(customerTwoUserName,
                                              customerTwoPassword,
                                              customerTwoFullName,
                                              customerTwoEmail,
                                              customerTwoPhoneNumber,
                                              new[] { customerRoleName });
                    }
                    if (customerThreeUserName != null && customerThreePassword != null && customerThreeFullName != null && customerThreeEmail != null && customerThreePhoneNumber != null)
                    {
                        await CreateUserAsync(customerThreeUserName,
                                              customerThreePassword,
                                              customerThreeFullName,
                                              customerThreeEmail,
                                              customerThreePhoneNumber,
                                              new[] { customerRoleName });
                    }
                    if (customerFourUserName != null && customerFourPassword != null && customerFourFullName != null && customerFourEmail != null && customerFourPhoneNumber != null)
                    {
                        await CreateUserAsync(customerFourUserName,
                                              customerFourPassword,
                                              customerFourFullName,
                                              customerFourEmail,
                                              customerFourPhoneNumber,
                                              new[] { customerRoleName });
                    }
                    if (customerFiveUserName != null && customerFivePassword != null && customerFiveFullName != null && customerFiveEmail != null && customerFivePhoneNumber != null)
                    {
                        await CreateUserAsync(customerFiveUserName,
                                              customerFivePassword,
                                              customerFiveFullName,
                                              customerFiveEmail,
                                              customerFivePhoneNumber,
                                              new[] { customerRoleName });
                    }
                }

                logger.LogInformation("Inbuilt account generation completed");
            }
        }

        private async Task EnsureRoleAsync(string roleName, string description, string[] claims)
        {
            if (await userRoleService.GetRoleByNameAsync(roleName) == null)
            {
                logger.LogInformation("Generating default role: {roleName}", roleName);

                var applicationRole = new ApplicationRole(roleName, description);

                var result = await userRoleService.CreateRoleAsync(applicationRole, claims);

                if (!result.Succeeded)
                {
                    throw new UserRoleException($"Seeding \"{description}\" role failed. Errors: " +
                        $"{string.Join(Environment.NewLine, result.Errors)}");
                }
            }
        }

        private async Task<ApplicationUser> CreateUserAsync(
            string userName, string password, string fullName, string email, string phoneNumber, string[] roles)
        {
            logger.LogInformation("Generating default user: {userName}", userName);

            var applicationUser = new ApplicationUser
            {
                UserName = userName,
                FullName = fullName,
                Email = email,
                PhoneNumber = phoneNumber,
                EmailConfirmed = true,
                IsEnabled = true
            };

            var result = await userAccountService.CreateUserAsync(applicationUser, roles, password);

            if (!result.Succeeded)
            {
                throw new UserAccountException($"Seeding \"{userName}\" user failed. Errors: " +
                    $"{string.Join(Environment.NewLine, result.Errors)}");
            }

            return applicationUser;
        }
    }
}
