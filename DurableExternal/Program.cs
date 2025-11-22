using Azure.Identity;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MyAzureFunctions;
using MyAzureFunctions.Activities;

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


    //builder..ConfigureAppConfiguration((context, configBuilder) =>
    //{
    //    var builtConfig = configBuilder.Build();
    //    var keyVaultEndpoint = builtConfig["AzureKeyVaultEndpoint"];

    //    if (!string.IsNullOrEmpty(keyVaultEndpoint))
    //    {
    //        // using Key Vault, either local dev or deployed
    //        configBuilder
    //            .SetBasePath(Environment.CurrentDirectory)
    //            .AddAzureKeyVault(new Uri(keyVaultEndpoint), new DefaultAzureCredential())
    //            .AddJsonFile("local.settings.json", optional: true)
    //            .AddEnvironmentVariables();
    //    }
    //    else
    //    {
    //        // local dev no Key Vault
    //        configBuilder
    //            .SetBasePath(Environment.CurrentDirectory)
    //            .AddJsonFile("local.settings.json", optional: true)
    //            .AddUserSecrets(Assembly.GetExecutingAssembly(), optional: true)
    //            .AddEnvironmentVariables();
    //    }
    //})
    //.Build();

builder.Build().Run();

