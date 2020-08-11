using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;
using DurableRetrySubOrchestrations.Model;
using System;

namespace DurableRetrySubOrchestrations.Orchestrations
{
    public class MySecondOrchestration
    {
        [FunctionName(Constants.MySecondOrchestration)]
        public async Task<MySubOrchestrationDto> RunOrchestrator(
            [OrchestrationTrigger] IDurableOrchestrationContext context,
            ILogger log)
        {
            var mySubOrchestrationDto = new MySubOrchestrationDto
            {
                InputStartData = context.GetInput<string>()
            };

            if (!context.IsReplaying)
            {
                log.LogWarning($"begin MySecondOrchestration with input {context.GetInput<string>()}");
            }

            var retryOptions = new RetryOptions(
                    firstRetryInterval: TimeSpan.FromSeconds(3),
                    maxNumberOfAttempts: 5)
            {
                BackoffCoefficient = 1.5
            };

            var myActivityThreeResult = await context.CallActivityWithRetryAsync<string>
                (Constants.MyActivityThree, retryOptions, context.GetInput<string>());

            mySubOrchestrationDto.MyActivityThreeResult = myActivityThreeResult;

            if(!context.IsReplaying)
            {
                log.LogWarning($"MySecondOrchestration MyActivityThree completed {myActivityThreeResult}");
            }

            var myActivityFourResult = await context.CallActivityAsync<string>(
                Constants.MyActivityFour, "MySecondOrchestration second Activity");

            mySubOrchestrationDto.MyActivityFourResult = myActivityFourResult;

            if (!context.IsReplaying)
            {
                log.LogWarning($"MySecondOrchestration MyActivityFour completed {myActivityFourResult}");
            }

            return mySubOrchestrationDto;
        }
    }
}