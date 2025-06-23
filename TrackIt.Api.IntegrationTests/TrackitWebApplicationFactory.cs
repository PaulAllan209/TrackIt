using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
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

                var migrationsAssembly = typeof(Program).GetTypeInfo().Assembly.GetName().Name;


                services.AddDbContext<ApplicationDbContext>(options =>
                {
                    options.UseSqlServer(connString, b => b.MigrationsAssembly(migrationsAssembly));

                    // Register the entity sets needed by OpenIddict.
                    options.UseOpenIddict();
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

            //return "Server=SUPERPOTATOPCV2\\TEW_SQLEXPRESS;Database=TrackItDB_test;Trusted_Connection=True;TrustServerCertificate=true;MultipleActiveResultSets=true";
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
