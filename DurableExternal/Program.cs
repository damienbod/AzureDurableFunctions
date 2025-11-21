using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using MyAzureFunctions.Activities;
using MyAzureFunctions;
using Azure.Identity;
using Microsoft.Extensions.Configuration;
using System.Reflection;

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication() // Using ASP.NET Core integration
    .ConfigureServices((context, services) =>
    {
        // Migrated from Startup.cs
        services.AddScoped<MyActivities>();

        services.AddOptions<MyConfiguration>()
            .Configure<IConfiguration>((settings, configuration) =>
            {
                configuration.GetSection("MyConfiguration").Bind(settings);
            });

        services.AddOptions<MyConfigurationSecrets>()
            .Configure<IConfiguration>((settings, configuration) =>
            {
                configuration.GetSection("MyConfigurationSecrets").Bind(settings);
            });
    })
    .ConfigureAppConfiguration((context, configBuilder) =>
    {
        var builtConfig = configBuilder.Build();
        var keyVaultEndpoint = builtConfig["AzureKeyVaultEndpoint"];

        if (!string.IsNullOrEmpty(keyVaultEndpoint))
        {
            // using Key Vault, either local dev or deployed
            configBuilder
                .SetBasePath(Environment.CurrentDirectory)
                .AddAzureKeyVault(new Uri(keyVaultEndpoint), new DefaultAzureCredential())
                .AddJsonFile("local.settings.json", optional: true)
                .AddEnvironmentVariables();
        }
        else
        {
            // local dev no Key Vault
            configBuilder
                .SetBasePath(Environment.CurrentDirectory)
                .AddJsonFile("local.settings.json", optional: true)
                .AddUserSecrets(Assembly.GetExecutingAssembly(), optional: true)
                .AddEnvironmentVariables();
        }
    })
    .Build();

host.Run();
