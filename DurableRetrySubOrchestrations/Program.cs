using Azure.Identity;
using DurableRetrySubOrchestrations;
using DurableRetrySubOrchestrations.Activities;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;
using Microsoft.Azure.Functions.Worker;

var builder = FunctionsApplication.CreateBuilder(args);

builder.ConfigureFunctionsWebApplication();

builder.Services
    .AddApplicationInsightsTelemetryWorkerService()
    .ConfigureFunctionsApplicationInsights();

builder.Services.AddScoped<MyActivities>();

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

builder.ConfigureFunctionsWebApplication();

var keyVaultEndpoint = builder.Configuration["AzureKeyVaultEndpoint"];

if (!string.IsNullOrEmpty(keyVaultEndpoint))
{
    // using Key Vault, either local dev or deployed
    builder.Configuration
        .SetBasePath(Environment.CurrentDirectory) //  .SetBasePath(context.ApplicationRootPath)
        .AddAzureKeyVault(new Uri(keyVaultEndpoint), new DefaultAzureCredential())
        .AddJsonFile("local.settings.json", optional: true)
        .AddEnvironmentVariables();
}
else
{
    // local dev no Key Vault
    builder.Configuration
        .SetBasePath(Environment.CurrentDirectory) //  .SetBasePath(context.ApplicationRootPath)
        .AddJsonFile("local.settings.json", optional: true)
        .AddUserSecrets(Assembly.GetExecutingAssembly(), optional: true)
        .AddEnvironmentVariables();
}

builder.Build().Run();