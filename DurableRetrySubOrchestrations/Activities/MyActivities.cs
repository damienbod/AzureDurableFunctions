using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Azure.Functions.Worker;

namespace DurableRetrySubOrchestrations.Activities
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
        public string MyActivityOne([ActivityTrigger] string input, ILogger log)
        {
            log.LogInformation($"Activity {Constants.MyActivityOne} {input} {_myConfiguration.Name} {_myConfigurationSecrets.MySecretOne} amount of retries: {_myConfiguration.AmountOfRetries}.");
            //throw new System.Exception("something went wrong");
            return $"{Constants.MyActivityOne} {input} {_myConfiguration.Name} {_myConfigurationSecrets.MySecretOne} amount of retries: {_myConfiguration.AmountOfRetries}.";
        }

        [Function(Constants.MyActivityTwo)]
        public string MyActivityTwo([ActivityTrigger] string input, ILogger log)
        {
            log.LogInformation($"Activity {Constants.MyActivityTwo}  {input} {_myConfiguration.Name}.");
            return $"{Constants.MyActivityTwo} {input} {_myConfiguration.Name}!";
        }

        [Function(Constants.MyActivityThree)]
        public string MyActivityThree([ActivityTrigger] string input, ILogger log)
        {
            log.LogInformation($"Activity {Constants.MyActivityThree}  {input} {_myConfiguration.Name}.");
            return $"{Constants.MyActivityThree} {input} {_myConfiguration.Name}!";
        }

        [Function(Constants.MyActivityFour)]
        public string MyActivityFour([ActivityTrigger] string input, ILogger log)
        {
            log.LogInformation($"Activity {Constants.MyActivityFour}  {input} {_myConfiguration.Name}.");
            //throw new System.Exception("something went wrong");
            return $"{Constants.MyActivityFour} {input} {_myConfiguration.Name}!";
        }
    }
}