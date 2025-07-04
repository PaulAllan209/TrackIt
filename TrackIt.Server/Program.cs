using AspNetCoreRateLimit;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Logging;
using Microsoft.OpenApi.Models;
using OpenIddict.Validation.AspNetCore;
using Serilog;
using TrackIt.Core.Infrastructure;
using TrackIt.Core.Interfaces;
using TrackIt.Core.Models.Account;
using TrackIt.Core.Services.Account;
using TrackIt.Server.Attributes;
using TrackIt.Server.Authorization;
using TrackIt.Server.Configuration;
using TrackIt.Server.Extensions;
using TrackIt.Server.Services;
using static OpenIddict.Abstractions.OpenIddictConstants;


var builder = WebApplication.CreateBuilder(args);

// FOR DOCKER ENVIRONMENT ONLY SETTING UP USER SECRETS
if (builder.Environment.EnvironmentName == "Docker")
{
    var args2 = Environment.GetCommandLineArgs();
    var secretsFilePath = builder.Configuration["SECRETS_FILE_PATH"] ??
                         (args2.Length > 0 && args2[0].StartsWith("--secretsfile=") ?
                          args2[0].Substring("--secretsfile=".Length) : null);

    if (!string.IsNullOrEmpty(secretsFilePath) && File.Exists(secretsFilePath))
    {
        builder.Configuration.AddJsonFile(secretsFilePath, optional: false, reloadOnChange: true);
    }
    else
    {
        Console.WriteLine("Docker environment detected but no secrets file found.");
    }
}

/************* ADD SERVICES *************/

// Configure Serilog
builder.Services.ConfigureSerilog(builder.Configuration); // In service extensions

// Configure sql server connection
builder.Services.ConfigureSqlContext(builder.Configuration); // In service extensions

// For Rate Limiting
builder.Services.AddMemoryCache();
builder.Services.ConfigureRateLimitingOptions(); // In service extensions
builder.Services.AddHttpContextAccessor();

// Register the Identity services.
builder.Services.AddIdentity<ApplicationUser, ApplicationRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// Configure Identity options and password complexity here
builder.Services.ConfigureIdentityOptions(); // In service extensions

// OpenIddict offers native integration with Quartz.NET to perform scheduled tasks
// Configure OpenIddict periodic pruning of orphaned authorizations/tokens from the database
builder.Services.ConfigureQuartz(); // In service extensions

builder.Services.ConfigureOpenIddict(); // In service extensions

builder.Services.ConfigureAuthInfrastructure(); // In service extensions

// Add cors
builder.Services.AddCors();

// Suppress the default automatic model state validation response.
// This allows custom validation handling (e.g., via ValidationFilterAttribute)
// so that model validation errors can be returned in a custom format.
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

// Action filters
// Uncomment this and delete the options in AddControllers if you want to manually add [ServiceFilter(typeof(ValidationFilterAttribute))] in controllers
builder.Services.AddScoped<ValidationFilterAttribute>();

// Newtonsoft Json is needed for patch requests specifically patch docs
builder.Services.AddControllers().AddNewtonsoftJson(); 

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    // Adds a Swagger document with metadata (title, version)
    c.SwaggerDoc("v1", new OpenApiInfo { Title = OidcServerConfig.ServerName, Version = "v1" });

    // Adds a custom operation filter to mark endpoints that require authorization
    // (e.g., adds a lock icon, 401 responses, and security requirements in the docs
    c.OperationFilter<SwaggerAuthorizeOperationFilter>();

    // Adds an OAuth2 security definition for the Swagger UI
    c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.OAuth2,
        Flows = new OpenApiOAuthFlows
        {
            Password = new OpenApiOAuthFlow
            {
                TokenUrl = new Uri("/connect/token", UriKind.Relative)
            }
        }
    });
});

builder.Services.AddAutoMapper(typeof(Program));

// Configurations

// Repositories
builder.Services.ConfigureRepositories();

// Business Services
builder.Services.ConfigureBusinessServices(); // In service extensions

// Other Services
builder.Services.AddScoped<IUserIdAccessor, UserIdAccessor>();

// DB Creation and Seeding
builder.Services.AddTransient<IDatabaseSeeder, DatabaseSeeder>();

//File Logger
builder.Host.UseSerilog();

var app = builder.Build();

// Global exception handling
var logger = app.Services.GetRequiredService<ILogger<Program>>();
app.ConfigureExceptionHandler(logger);

/************* CONFIGURE REQUEST PIPELINE *************/

// Enables serving static files (e.g., index.html, CSS, JS) from wwwroot or the configured static files directory.
// UseDefaultFiles() rewrites requests to the default file (like index.html) if no file is specified in the URL.
// MapStaticAssets() is a custom extension (likely wraps UseStaticFiles) to serve static assets for the SPA frontend.
app.UseDefaultFiles();
app.MapStaticAssets();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.DocumentTitle = "Swagger UI - TrackIt";
        c.SwaggerEndpoint("/swagger/v1/swagger.json", $"{OidcServerConfig.ServerName} V1");
        c.OAuthClientId(OidcServerConfig.SwaggerClientID);
    });

    IdentityModelEventSource.ShowPII = true;
}
else
{
    // The default HSTS value is 30 days.
    // You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseIpRateLimiting();

app.UseCors(builder => builder
    .AllowAnyOrigin()
    .AllowAnyHeader()
    .AllowAnyMethod());

// Use security headers middleware
app.UseMiddleware<SecurityHeadersMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("/index.html");

/************* SEED DATABASE *************/

using var scope = app.Services.CreateScope();
try
{
    var dbSeeder = scope.ServiceProvider.GetRequiredService<IDatabaseSeeder>();
    await dbSeeder.SeedAsync();

    await OidcServerConfig.RegisterClientApplicationsAsync(scope.ServiceProvider);
}
catch (Exception ex)
{
    logger.LogCritical(ex, "An error occurred whilst creating/seeding database");

    throw;
}

/************* RUN APP *************/

app.Run();

