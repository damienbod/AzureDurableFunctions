using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using DurableRetrySubOrchestrations.Model;
using System;
using Microsoft.Azure.Functions.Worker;

namespace DurableRetrySubOrchestrations.Orchestrations
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

            var retryOptions = new RetryOptions(
                    firstRetryInterval: TimeSpan.FromSeconds(3),
                    maxNumberOfAttempts: 5)
            {
                BackoffCoefficient = 1.5
            };

            var myActivityOne = await context.CallActivityWithRetryAsync<string>
                (Constants.MyActivityOne, retryOptions, context.GetInput<string>());

            myOrchestrationDto.MyActivityOneResult = myActivityOne;

            if(!context.IsReplaying)
            {
                log.LogWarning($"myActivityOne completed {myActivityOne}");
            }

            var mySubOrchestrationDto = await context.CallSubOrchestratorWithRetryAsync<MySubOrchestrationDto>
               (Constants.MySecondOrchestration, retryOptions, myActivityOne);

            myOrchestrationDto.MySubOrchestration = mySubOrchestrationDto;

            if (!context.IsReplaying)
            {
                log.LogWarning($"mySubOrchestrationDto completed {mySubOrchestrationDto.MyActivityThreeResult}");
            }

            var myActivityTwo = await context.CallActivityAsync<string>(
                Constants.MyActivityTwo, "Start MyOrchestration Activity 2");

            myOrchestrationDto.MyActivityTwoResult = myActivityTwo;

            if (!context.IsReplaying)
            {
                log.LogWarning($"myActivityTwo completed {myActivityTwo}");
            }

            return myOrchestrationDto;
        }
    }
}