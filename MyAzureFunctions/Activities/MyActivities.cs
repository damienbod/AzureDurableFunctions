using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

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

        [FunctionName(Constants.MyActivityOne)]
        public string MyActivityOne([ActivityTrigger] IDurableActivityContext context, ILogger log)
        {
            string name = context.GetInput<string>();
            log.LogInformation($"Activity {Constants.MyActivityOne} {name} {_myConfiguration.Name} amount of retries: {_myConfiguration.AmountOfRetries}.");
            return $"{Constants.MyActivityOne}  {name} {_myConfiguration.Name}!";
        }

        [FunctionName(Constants.MyActivityTwo)]
        public string MyActivityTwo([ActivityTrigger] IDurableActivityContext context, ILogger log)
        {
            string name = context.GetInput<string>();
            log.LogInformation($"Activity {Constants.MyActivityTwo}  {name} {_myConfiguration.Name}.");
            return $"{Constants.MyActivityTwo}  {name} {_myConfiguration.Name}!";
        }

    }
}