using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using DurableWait;
using DurableWait.Activities;
using System;
using System.Reflection;
using Azure.Identity;

[assembly: FunctionsStartup(typeof(Startup))]

namespace DurableWait
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
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
        }

        public override void ConfigureAppConfiguration(IFunctionsConfigurationBuilder builder)
        {
            var builtConfig = builder.ConfigurationBuilder.Build();
            var keyVaultEndpoint = builtConfig["AzureKeyVaultEndpoint"];

            if (!string.IsNullOrEmpty(keyVaultEndpoint))
            {
                // might need this depending on local dev env
                //var credential = new DefaultAzureCredential(
                //    new DefaultAzureCredentialOptions { ExcludeSharedTokenCacheCredential = true });

                // using Key Vault, either local dev or deployed
                builder.ConfigurationBuilder
                        .SetBasePath(Environment.CurrentDirectory)
                        .AddAzureKeyVault(new Uri(keyVaultEndpoint), new DefaultAzureCredential())
                        .AddJsonFile("local.settings.json", true)
                        .AddEnvironmentVariables()
                    .Build();
            }
            else
            {
                // local dev no Key Vault
                builder.ConfigurationBuilder
                   .SetBasePath(Environment.CurrentDirectory)
                   .AddJsonFile("local.settings.json", true)
                   .AddUserSecrets(Assembly.GetExecutingAssembly(), true)
                   .AddEnvironmentVariables()
                   .Build();
            }
        }
    }
}