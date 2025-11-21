using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using DurableRetrySubOrchestrations.Model;
using System;
using Microsoft.Azure.Functions.Worker;
using Microsoft.DurableTask;

namespace DurableRetrySubOrchestrations.Orchestrations;

public class MyOrchestration
{
    [Function(Constants.MyOrchestration)]
    public async Task<MyOrchestrationDto> RunOrchestrator(
        [OrchestrationTrigger] TaskOrchestrationContext context)
    {
        var log = context.CreateReplaySafeLogger<MyOrchestration>();
        
        var myOrchestrationDto = new MyOrchestrationDto
        {
            InputStartData = context.GetInput<string>()
        };

        if (!context.IsReplaying)
        {
            log.LogWarning($"begin MyOrchestration with input {context.GetInput<string>()}");
        }

        var retryOptions = new TaskOptions(
            new RetryPolicy(
                maxNumberOfAttempts: 5,
                firstRetryInterval: TimeSpan.FromSeconds(3),
                backoffCoefficient: 1.5));

        var myActivityOne = await context.CallActivityAsync<string>
            (Constants.MyActivityOne, context.GetInput<string>(), retryOptions);

        myOrchestrationDto.MyActivityOneResult = myActivityOne;

        if(!context.IsReplaying)
        {
            log.LogWarning($"myActivityOne completed {myActivityOne}");
        }

        var mySubOrchestrationDto = await context.CallSubOrchestratorAsync<MySubOrchestrationDto>
           (Constants.MySecondOrchestration, myActivityOne, retryOptions);

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