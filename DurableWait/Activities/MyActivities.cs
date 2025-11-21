using DurableWait.Model;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Threading;
using Microsoft.Azure.Functions.Worker;

namespace DurableWait.Activities
{
    public class MyActivities
    {
        private readonly MyConfiguration _myConfiguration;
        private readonly MyConfigurationSecrets _myConfigurationSecrets;

        public MyActivities(IOptions<MyConfiguration> myConfiguration, 
            IOptions<MyConfigurationSecrets> myConfigurationSecrets)
        {
            _myConfiguration = myConfiguration.Value;
            _myConfigurationSecrets = myConfigurationSecrets.Value;
        }

        [Function(Constants.MyActivityOne)]
        public string MyActivityOne([ActivityTrigger] IDurableActivityContext context, ILogger log)
        {
            BeginRequestData beginRequestData = context.GetInput<BeginRequestData>();
            log.LogInformation($"Activity {Constants.MyActivityOne} {beginRequestData.Id} {_myConfiguration.Name} {_myConfigurationSecrets.MySecretOne} amount of retries: {_myConfiguration.AmountOfRetries}.");
            return $"{Constants.MyActivityOne} {beginRequestData.Id} {_myConfiguration.Name} {_myConfigurationSecrets.MySecretOne} amount of retries: {_myConfiguration.AmountOfRetries}.";
        }

        [Function(Constants.MyActivityTwo)]
        public string MyActivityTwo([ActivityTrigger] IDurableActivityContext context, ILogger log)
        {
            // simi HTTP request which lasts 14s and causes timeout
            // Thread.Sleep(14000);

            MyOrchestrationDto myOrchestrationDto = context.GetInput<MyOrchestrationDto>();
            log.LogInformation($"Activity {Constants.MyActivityTwo}  {myOrchestrationDto.BeginRequest.Message} {_myConfiguration.Name}.");
            return $"{Constants.MyActivityTwo} {myOrchestrationDto.BeginRequest.Message} {_myConfiguration.Name}!";
        }

    }
}