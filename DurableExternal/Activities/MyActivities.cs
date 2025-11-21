using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Azure.Functions.Worker;

namespace MyAzureFunctions.Activities
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
        public string MyActivityOne([ActivityTrigger] string name, ILogger log)
        {
            log.LogInformation($"Activity {Constants.MyActivityOne} {name} {_myConfiguration.Name} {_myConfigurationSecrets.MySecretOne} amount of retries: {_myConfiguration.AmountOfRetries}.");
            return $"{Constants.MyActivityOne} {name} {_myConfiguration.Name} {_myConfigurationSecrets.MySecretOne} amount of retries: {_myConfiguration.AmountOfRetries}.";
        }

        [Function(Constants.MyActivityTwo)]
        public string MyActivityTwo([ActivityTrigger] string name, ILogger log)
        {
            log.LogInformation($"Activity {Constants.MyActivityTwo}  {name} {_myConfiguration.Name}.");
            return $"{Constants.MyActivityTwo} {name} {_myConfiguration.Name}!";
        }

    }
}