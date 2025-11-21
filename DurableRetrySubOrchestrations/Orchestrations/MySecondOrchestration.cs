using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using DurableRetrySubOrchestrations.Model;
using System;
using Microsoft.Azure.Functions.Worker;
using Microsoft.DurableTask;

namespace DurableRetrySubOrchestrations.Orchestrations;

public class MySecondOrchestration
{
    [Function(Constants.MySecondOrchestration)]
    public async Task<MySubOrchestrationDto> RunOrchestrator(
        [OrchestrationTrigger] TaskOrchestrationContext context)
    {
        var log = context.CreateReplaySafeLogger<MySecondOrchestration>();
        
        var mySubOrchestrationDto = new MySubOrchestrationDto
        {
            InputStartData = context.GetInput<string>()
        };

        if (!context.IsReplaying)
        {
            log.LogWarning($"begin MySecondOrchestration with input {context.GetInput<string>()}");
        }

        var retryOptions = new TaskOptions(
            new RetryPolicy(
                maxNumberOfAttempts: 5,
                firstRetryInterval: TimeSpan.FromSeconds(3),
                backoffCoefficient: 1.5));

        var myActivityThreeResult = await context.CallActivityAsync<string>
            (Constants.MyActivityThree, context.GetInput<string>(), retryOptions);

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