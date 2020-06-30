using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;

namespace MyAzureFunctions.Activities
{
    public class MyActivities
    {
        [FunctionName(Constants.MyActivityOne)]
        public string MyActivityOne([ActivityTrigger] IDurableActivityContext context, ILogger log)
        {
            string name = context.GetInput<string>();
            log.LogInformation($"Activity {Constants.MyActivityOne} {name}.");
            return $"{Constants.MyActivityOne} {name}!";
        }

        [FunctionName(Constants.MyActivityTwo)]
        public string MyActivityTwo([ActivityTrigger] IDurableActivityContext context, ILogger log)
        {
            string name = context.GetInput<string>();
            log.LogInformation($"Activity {Constants.MyActivityTwo} {name}.");
            return $"{Constants.MyActivityTwo} {name}!";
        }

    }
}