using DurableWait;
using DurableWait.Model;
using Microsoft.Azure.Functions.Worker;
using Microsoft.DurableTask;
using Microsoft.Extensions.Logging;

namespace MyAzureFDurableWaitunctions.Orchestrations;

public class MyOrchestration
{
    private readonly ILogger<MyOrchestration> _logger;

    public MyOrchestration(ILogger<MyOrchestration> logger)
    {
        _logger = logger;
    }

    [Function(Constants.MyOrchestration)]
    public async Task<MyOrchestrationDto> RunOrchestrator(
        [OrchestrationTrigger] TaskOrchestrationContext context)
    {
        var myOrchestrationDto = new MyOrchestrationDto
        {
            BeginRequest = context.GetInput<BeginRequestData>()
        };

        if (!context.IsReplaying)
        {
            _logger.LogWarning("begin MyOrchestration with input id {myOrchestrationDtoBeginRequestId}", myOrchestrationDto.BeginRequest.Id);
        }

        var myActivityOne = await context.CallActivityAsync<string>(
            Constants.MyActivityOne, context.GetInput<BeginRequestData>());

        myOrchestrationDto.MyActivityOneResult = myActivityOne;

        if (!context.IsReplaying)
        {
            _logger.LogWarning("myActivityOne completed {myActivityOne}", myActivityOne);
        }

        var myActivityTwo = await context.CallActivityAsync<string>(
            Constants.MyActivityTwo, myOrchestrationDto);

        myOrchestrationDto.MyActivityTwoResult = myActivityTwo;

        if (!context.IsReplaying)
        {
            _logger.LogWarning("myActivityTwo completed {myActivityTwo}", myActivityTwo);
        }

        return myOrchestrationDto;
    }
}