using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using MyAzureFunctions;
using MyAzureFunctions.Activities;

[assembly: FunctionsStartup(typeof(Startup))]

namespace MyAzureFunctions
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            //var tableStorageConnection = Environment.GetEnvironmentVariable("connection...");        
            builder.Services.AddScoped<MyActivities>();
        }
    }
}