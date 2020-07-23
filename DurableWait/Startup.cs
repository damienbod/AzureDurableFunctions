using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using DurableWait;
using DurableWait.Activities;
using System;
using System.Reflection;

[assembly: FunctionsStartup(typeof(Startup))]

namespace DurableWait
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var keyVaultEndpoint = Environment.GetEnvironmentVariable("AzureKeyVaultEndpoint");

            if (!string.IsNullOrEmpty(keyVaultEndpoint))
            {
                // using Key Vault, either local dev or deployed
                var azureServiceTokenProvider = new AzureServiceTokenProvider();
                var keyVaultClient = new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(azureServiceTokenProvider.KeyVaultTokenCallback));

                var config = new ConfigurationBuilder()
                        .AddAzureKeyVault(keyVaultEndpoint)
                        .SetBasePath(Environment.CurrentDirectory)
                        .AddJsonFile("local.settings.json", true)
                        .AddEnvironmentVariables()
                    .Build();

                builder.Services.AddSingleton<IConfiguration>(config);
            }
            else
            {
                // local dev no Key Vault
                var config = new ConfigurationBuilder()
               .SetBasePath(Environment.CurrentDirectory)
               .AddJsonFile("local.settings.json", true)
               .AddUserSecrets(Assembly.GetExecutingAssembly(), true)
               .AddEnvironmentVariables()
               .Build();

                builder.Services.AddSingleton<IConfiguration>(config);
            }

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
    }
}