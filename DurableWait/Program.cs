using Azure.Identity;
using DurableWait;
using DurableWait.Activities;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;

var builder = FunctionsApplication.CreateBuilder(args);

builder.ConfigureFunctionsWebApplication();

builder.Services
    .AddApplicationInsightsTelemetryWorkerService()
    .ConfigureFunctionsApplicationInsights();

builder.Services.AddOptions<MyConfiguration>()
    .Configure<IConfiguration>((settings, configuration) =>
    {
        configuration.GetSection("MyConfiguration").Bind(settings);
    });

builder.Services.AddOptions<MyConfigurationSecrets>()
    .Configure<IConfiguration>((settings, configuration) =>
    {
        configuration.GetSection("MyConfigurationSecrets").Bind(settings);
    });

builder.Services.AddLogging();
builder.Services.AddScoped<MyActivities>();
builder.Services.AddScoped<Processing>();

var keyVaultEndpoint = builder.Configuration["AzureKeyVaultEndpoint"];

if (!string.IsNullOrEmpty(keyVaultEndpoint))
{
    // using Key Vault, either local dev or deployed
    builder.Configuration
        .SetBasePath(Environment.CurrentDirectory)
        .AddAzureKeyVault(new Uri(keyVaultEndpoint), new DefaultAzureCredential())
        .AddJsonFile("local.settings.json", optional: true)
        .AddEnvironmentVariables();
}
else
{
    // local dev no Key Vault
    builder.Configuration
        .SetBasePath(Environment.CurrentDirectory)
        .AddJsonFile("local.settings.json", optional: true)
        .AddUserSecrets(Assembly.GetExecutingAssembly(), optional: true)
        .AddEnvironmentVariables();
}

builder.Build().Run();