using AspNetCoreRateLimit;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OpenIddict.Validation.AspNetCore;
using Quartz;
using Serilog;
using System.Reflection;
using TrackIt.Core.Infrastructure;
using TrackIt.Core.Infrastructure.Repositories;
using TrackIt.Core.Interfaces.Repository;
using TrackIt.Core.Interfaces.Services;
using TrackIt.Core.Services;
using TrackIt.Core.Services.Account;
using TrackIt.Server.Authorization;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace TrackIt.Server.Extensions
{
    /// <summary>
    /// A class containing additional service to be registered in program.cs.
    /// Helps in maintaining cleanliness of program.cs
    /// </summary>
    public static class ServiceExtensions
    {
        public static void ConfigureSerilog(this IServiceCollection services, IConfiguration configuration)
        {
            Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .CreateLogger();
        }

        public static void ConfigureSqlContext(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection") ??
                throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

            var migrationsAssembly = typeof(Program).GetTypeInfo().Assembly.GetName().Name;

            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(connectionString, b => b.MigrationsAssembly(migrationsAssembly));

                // Register the entity sets needed by OpenIddict.
                options.UseOpenIddict();
            });
        }

        public static void ConfigureRateLimitingOptions(this IServiceCollection services)
        {
            var rateLimitRules = new List<RateLimitRule>
            {
                new RateLimitRule
                {
                    Endpoint = "*",
                    Limit = 1000,
                    Period = "2m"
                }
            };

            services.Configure<IpRateLimitOptions>(opt =>
            {
                opt.GeneralRules = rateLimitRules;
            });
            services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
            services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
            services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
            services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();
        }

        public static void ConfigureIdentityOptions(this IServiceCollection services)
        {
            services.Configure<IdentityOptions>(options =>
            {
                // User settings
                options.User.RequireUniqueEmail = true;
                options.Password.RequireDigit = true;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = false;

                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
                options.Lockout.MaxFailedAccessAttempts = 10;

                // Password settings
                /*
                options.Password.RequiredLength = 8;

                // Lockout settings
                */

                // Configure Identity to use the same JWT claims as OpenIddict
                options.ClaimsIdentity.UserNameClaimType = Claims.Name;
                options.ClaimsIdentity.UserIdClaimType = Claims.Subject;
                options.ClaimsIdentity.RoleClaimType = Claims.Role;
                options.ClaimsIdentity.EmailClaimType = Claims.Email;
            });
        }

        public static void ConfigureQuartz(this IServiceCollection services)
        {
            services.AddQuartz(options =>
            {
                options.UseSimpleTypeLoader();
                options.UseInMemoryStore();
            });

            // Register the Quartz.NET service and configure it to block shutdown until jobs are complete.
            services.AddQuartzHostedService(options => options.WaitForJobsToComplete = true);
        }

        public static void ConfigureOpenIddict(this IServiceCollection services)
        {
            services.AddOpenIddict()
                .AddCore(options =>
                {
                    options.UseEntityFrameworkCore()
                            .UseDbContext<ApplicationDbContext>();

                    options.UseQuartz();
                })
                .AddServer(options =>
                {
                    // Enable the token endpoint
                    options.SetTokenEndpointUris("connect/token");

                    options.AllowPasswordFlow()
                            .AllowRefreshTokenFlow();

                    options.RegisterScopes(
                        Scopes.Profile,
                        Scopes.Email,
                        Scopes.Address,
                        Scopes.Phone,
                        Scopes.Roles);

                    // Register the signing and encryption credentials.
                    options.AddDevelopmentEncryptionCertificate()
                            .AddDevelopmentSigningCertificate();

                    // Register the ASP.NET Core host and configure the ASP.NET Core-specific options.
                    options.UseAspNetCore()
                            .EnableTokenEndpointPassthrough();
                })
                // Register the OpenIddict validation components.
                .AddValidation(options =>
                {
                    // Import the configuration from the local OpenIddict server instance.
                    options.UseLocalServer();

                    // Register the ASP.NET Core host.
                    options.UseAspNetCore();
                });
        }

        public static void ConfigureAuthInfrastructure(this IServiceCollection services)
        {
            services.AddAuthentication(o =>
            {
                o.DefaultScheme = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme;
                o.DefaultAuthenticateScheme = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme;
                o.DefaultChallengeScheme = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme;
            });

            services.AddAuthorizationBuilder()
                .AddPolicy(AuthPolicies.ViewAllUsersPolicy,
                    policy => policy.RequireClaim(CustomClaims.Permission, ApplicationPermissions.ViewUsers))
                .AddPolicy(AuthPolicies.ManageAllUsersPolicy,
                    policy => policy.RequireClaim(CustomClaims.Permission, ApplicationPermissions.ManageUsers))
                .AddPolicy(AuthPolicies.ViewAllRolesPolicy,
                    policy => policy.RequireClaim(CustomClaims.Permission, ApplicationPermissions.ViewRoles))
                .AddPolicy(AuthPolicies.ViewRoleByRoleNamePolicy,
                    policy => policy.RequireClaim(CustomClaims.Permission, ApplicationPermissions.ManageRoles))
                .AddPolicy(AuthPolicies.ManageAllRolesPolicy,
                    policy => policy.RequireClaim(CustomClaims.Permission, ApplicationPermissions.ManageRoles))
                .AddPolicy(AuthPolicies.AssignAllowedRolesPolicy,
                    policy => policy.RequireClaim(CustomClaims.Permission, ApplicationPermissions.ManageRoles))

                // Shipment Policies
                .AddPolicy(AuthPolicies.CreateShipmentPolicy,
                    policy => policy.RequireClaim(CustomClaims.Permission, ApplicationPermissions.CreateShipment))
                .AddPolicy(AuthPolicies.ViewShipmentPolicy,
                    policy => policy.RequireClaim(CustomClaims.Permission, ApplicationPermissions.ViewShipment))
                .AddPolicy(AuthPolicies.UpdateShipmentPolicy,
                    policy => policy.RequireClaim(CustomClaims.Permission, ApplicationPermissions.UpdateShipment))
                .AddPolicy(AuthPolicies.DeleteShipmentPolicy,
                    policy => policy.RequireClaim(CustomClaims.Permission, ApplicationPermissions.DeleteShipment))
                
                // Status update policies
                .AddPolicy(AuthPolicies.CreateStatusPolicy,
                    policy => policy.RequireClaim(CustomClaims.Permission, ApplicationPermissions.CreateStatus))
                .AddPolicy(AuthPolicies.UpdateStatusPolicy,
                    policy => policy.RequireClaim(CustomClaims.Permission, ApplicationPermissions.UpdateStatus))
                .AddPolicy(AuthPolicies.ViewStatusHistoryPolicy,
                    policy => policy.RequireClaim(CustomClaims.Permission, ApplicationPermissions.ViewStatusHistory))

                // Role-specific policies
                .AddPolicy(AuthPolicies.SupplierOperationsPolicy,
                    policy => policy.RequireClaim(CustomClaims.Permission, 
                    [
                        ApplicationPermissions.CreateShipment,
                        ApplicationPermissions.ViewShipment,
                        ApplicationPermissions.UpdateShipment,
                        ApplicationPermissions.DeleteShipment,
                        ApplicationPermissions.ViewStatusHistory,
                        ApplicationPermissions.CreateStatus,
                        ApplicationPermissions.UpdateStatus
                    ]))
                .AddPolicy(AuthPolicies.FacilityOperationsPolicy, 
                    policy => policy.RequireClaim(CustomClaims.Permission,
                    [
                        ApplicationPermissions.ViewShipment,
                        ApplicationPermissions.UpdateStatus,
                        ApplicationPermissions.ViewStatusHistory
                    ]))
                .AddPolicy(AuthPolicies.DeliveryOperationsPolicy,
                    policy => policy.RequireClaim(CustomClaims.Permission,
                    [
                        ApplicationPermissions.ViewShipment,
                        ApplicationPermissions.UpdateStatus,
                        ApplicationPermissions.ViewStatusHistory
                    ]))
                .AddPolicy(AuthPolicies.CustomerOperationsPolicy,
                    policy => policy.RequireClaim(CustomClaims.Permission,
                    [
                        ApplicationPermissions.ViewShipment,
                        ApplicationPermissions.ViewStatusHistory,
                        ApplicationPermissions.SetStatusDelivered
                    ]))
                ;
                


        }

        public static void ConfigureRepositories(this IServiceCollection services)
        {
            services.AddScoped<IShipmentRepository, ShipmentRepository>();
        }

        public static void ConfigureBusinessServices(this IServiceCollection services)
        {
            services.AddScoped<IUserAccountService, UserAccountService>();
            services.AddScoped<IUserRoleService, UserRoleService>();

            services.AddScoped<IShipmentService, ShipmentService>();
        }
    }
}
