using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MyAzureFunctions.Model;
using Microsoft.Azure.Functions.Worker;

namespace MyAzureFunctions.Orchestrations
{
    public class MyOrchestration
    {
        [Function(Constants.MyOrchestration)]
        public async Task<MyOrchestrationDto> RunOrchestrator(
            [OrchestrationTrigger] IDurableOrchestrationContext context,
            ILogger log)
        {
            var myOrchestrationDto = new MyOrchestrationDto
            {
                InputStartData = context.GetInput<string>()
            };

            if (!context.IsReplaying)
            {
                log.LogWarning($"begin MyOrchestration with input {context.GetInput<string>()}");
            }

            var myActivityOne = await context.CallActivityAsync<string>(
                Constants.MyActivityOne, context.GetInput<string>());

            myOrchestrationDto.MyActivityOneResult = myActivityOne;

            if(!context.IsReplaying)
            {
                log.LogWarning($"myActivityOne completed {myActivityOne}");
            }

            var myActivityTwoInputEvent = await context.WaitForExternalEvent<string>(
                Constants.MyExternalInputEvent);
            myOrchestrationDto.ExternalInputData = myActivityTwoInputEvent;

            var myActivityTwo = await context.CallActivityAsync<string>(
                Constants.MyActivityTwo, myActivityTwoInputEvent);

            myOrchestrationDto.MyActivityTwoResult = myActivityTwo;

            if (!context.IsReplaying)
            {
                log.LogWarning($"myActivityTwo completed {myActivityTwo}");
            }

            return myOrchestrationDto;
        }
    }
}