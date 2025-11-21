using Microsoft.Azure.Functions.Worker;
using Microsoft.DurableTask;
using Microsoft.Extensions.Logging;
using MyAzureFunctions.Model;

namespace MyAzureFunctions.Orchestrations;

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

        var myActivityOne = await context.CallActivityAsync<string>(
            Constants.MyActivityOne, context.GetInput<string>());

        myOrchestrationDto.MyActivityOneResult = myActivityOne;

        if (!context.IsReplaying)
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