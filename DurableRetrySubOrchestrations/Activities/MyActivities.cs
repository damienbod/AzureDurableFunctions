using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Extensions.DurableTask;
using Microsoft.DurableTask;

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
        public string MyActivityOne([ActivityTrigger] TaskActivityContext context, ILogger log)
        {
            string name = context.GetInput<string>();
            log.LogInformation($"Activity {Constants.MyActivityOne} {name} {_myConfiguration.Name} {_myConfigurationSecrets.MySecretOne} amount of retries: {_myConfiguration.AmountOfRetries}.");
            //throw new System.Exception("something went wrong");
            return $"{Constants.MyActivityOne} {name} {_myConfiguration.Name} {_myConfigurationSecrets.MySecretOne} amount of retries: {_myConfiguration.AmountOfRetries}.";
        }

        [Function(Constants.MyActivityTwo)]
        public string MyActivityTwo([ActivityTrigger] TaskActivityContext context, ILogger log)
        {
            string name = context.GetInput<string>();
            log.LogInformation($"Activity {Constants.MyActivityTwo}  {name} {_myConfiguration.Name}.");
            return $"{Constants.MyActivityTwo} {name} {_myConfiguration.Name}!";
        }

        [Function(Constants.MyActivityThree)]
        public string MyActivityThree([ActivityTrigger] TaskActivityContext context, ILogger log)
        {
            string name = context.GetInput<string>();
            log.LogInformation($"Activity {Constants.MyActivityThree}  {name} {_myConfiguration.Name}.");
            return $"{Constants.MyActivityThree} {name} {_myConfiguration.Name}!";
        }

        [Function(Constants.MyActivityFour)]
        public string MyActivityFour([ActivityTrigger] TaskActivityContext context, ILogger log)
        {
            string name = context.GetInput<string>();
            log.LogInformation($"Activity {Constants.MyActivityFour}  {name} {_myConfiguration.Name}.");
            //throw new System.Exception("something went wrong");
            return $"{Constants.MyActivityFour} {name} {_myConfiguration.Name}!";
        }
    }
}