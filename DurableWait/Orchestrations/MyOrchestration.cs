using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using DurableWait.Model;
using DurableWait;
using Microsoft.Azure.Functions.Worker;
using Microsoft.DurableTask;

namespace MyAzureFDurableWaitunctions.Orchestrations;

public class MyOrchestration
{
    [Function(Constants.MyOrchestration)]
    public async Task<MyOrchestrationDto> RunOrchestrator(
        [OrchestrationTrigger] TaskOrchestrationContext context)
    {
        var log = context.CreateReplaySafeLogger<MyOrchestration>();
        
        var myOrchestrationDto = new MyOrchestrationDto
        {
            BeginRequest = context.GetInput<BeginRequestData>()
        };

        if (!context.IsReplaying)
        {
            log.LogWarning($"begin MyOrchestration with input id {myOrchestrationDto.BeginRequest.Id}");
        }

        var myActivityOne = await context.CallActivityAsync<string>(
            Constants.MyActivityOne, context.GetInput<BeginRequestData>());

        myOrchestrationDto.MyActivityOneResult = myActivityOne;

        if(!context.IsReplaying)
        {
            log.LogWarning($"myActivityOne completed {myActivityOne}");
        }

        var myActivityTwo = await context.CallActivityAsync<string>(
            Constants.MyActivityTwo, myOrchestrationDto);

        myOrchestrationDto.MyActivityTwoResult = myActivityTwo;

        if (!context.IsReplaying)
        {
            log.LogWarning($"myActivityTwo completed {myActivityTwo}");
        }

        return myOrchestrationDto;
    }
}