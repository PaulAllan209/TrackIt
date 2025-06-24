using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations.Internal;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using OpenIddict.Server;
using OpenIddict.Server.AspNetCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TrackIt.Core.Infrastructure;

namespace TrackIt.Api.IntegrationTests
{
    internal class TrackItWebApplicationFactory : WebApplicationFactory<Program>
    {
        private readonly string _dbNameSuffix;

        public TrackItWebApplicationFactory(string dbNameSuffix)
        {
            _dbNameSuffix = dbNameSuffix;
        }
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureAppConfiguration((context, config) =>
            {
                config.AddUserSecrets<TrackItWebApplicationFactory>();
            });

            builder.ConfigureTestServices(services =>
            {
                services.RemoveAll(typeof(DbContextOptions<ApplicationDbContext>));

                var connString = GetConnectionString();
                // Add unique suffix to database name
                connString = ModifyConnectionStringWithUniqueName(connString);

                var migrationsAssembly = typeof(Program).GetTypeInfo().Assembly.GetName().Name;


                services.AddDbContext<ApplicationDbContext>(options =>
                {
                    options.UseSqlServer(connString, b => b.MigrationsAssembly(migrationsAssembly));

                    // Register the entity sets needed by OpenIddict.
                    options.UseOpenIddict();
                });

                // Also configure the ASP.NET Core integration
                services.Configure<OpenIddictServerAspNetCoreOptions>(options =>
                {
                    options.DisableTransportSecurityRequirement = true; // This allows HTTP
                });

                var dbContext = CreateDbContext(services);
                dbContext.Database.EnsureDeleted();
            });
        }

        private static string? GetConnectionString()
        {
            // Typically you use user secrets when doing connection string but im not doing it here
            // Just uncomment these and setup user secrets if you need to customize

            var configuration = new ConfigurationBuilder()
                .AddUserSecrets<TrackItWebApplicationFactory>()
                .Build();

            var connString = configuration.GetConnectionString("TrackIt");
            return connString;
        }

        private string ModifyConnectionStringWithUniqueName(string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))
                return connectionString;

            var builder = new SqlConnectionStringBuilder(connectionString)
            {
                InitialCatalog = $"TrackItDB_test_{_dbNameSuffix}"
            };

            return builder.ConnectionString;
        }

        private static ApplicationDbContext CreateDbContext(IServiceCollection services)
        {
            var serviceProvider = services.BuildServiceProvider();
            var scope = serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            return dbContext;
        }
    }
}
