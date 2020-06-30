using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;

namespace MyAzureFunctions.Orchestrationss
{
    public class MyOrchestration
    {
        [FunctionName(Constants.MyOrchestration)]
        public async Task<List<string>> RunOrchestrator(
            [OrchestrationTrigger] IDurableOrchestrationContext context,
            ILogger log)
        {
            var outputs = new List<string>();

            if (!context.IsReplaying)
            {
                log.LogWarning($"begin MyOrchestration with input {context.GetInput<string>()}");
            }

            var myActivityOne = await context.CallActivityAsync<string>(
                Constants.MyActivityOne, context.GetInput<string>());

            outputs.Add(myActivityOne);

            if(!context.IsReplaying)
            {
                log.LogWarning($"myActivityOne completed {myActivityOne}");
            }

            var myActivityTwoInputEvent = await context.WaitForExternalEvent<string>(
                Constants.MyExternalInputEvent);

            var myActivityTwo = await context.CallActivityAsync<string>(
                Constants.MyActivityTwo, myActivityTwoInputEvent);

            outputs.Add(myActivityTwo);

            if (!context.IsReplaying)
            {
                log.LogWarning($"myActivityTwo completed {myActivityTwo}");
            }

            return outputs;
        }
    }
}