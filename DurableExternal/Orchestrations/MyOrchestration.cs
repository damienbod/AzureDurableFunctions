using Microsoft.Azure.Functions.Worker;
using Microsoft.DurableTask;
using Microsoft.Extensions.Logging;
using MyAzureFunctions.Apis;
using MyAzureFunctions.Model;

namespace MyAzureFunctions.Orchestrations;

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
            InputStartData = context.GetInput<string>()
        };

        if (!context.IsReplaying)
        {
            _logger.LogWarning("begin MyOrchestration with input {input}", context.GetInput<string>());
        }

        var myActivityOne = await context.CallActivityAsync<string>(
            Constants.MyActivityOne, context.GetInput<string>());

        myOrchestrationDto.MyActivityOneResult = myActivityOne;

        if (!context.IsReplaying)
        {
            _logger.LogWarning("myActivityOne completed {myActivityOne}", myActivityOne);
        }

        var myActivityTwoInputEvent = await context.WaitForExternalEvent<string>(
            Constants.MyExternalInputEvent);
        myOrchestrationDto.ExternalInputData = myActivityTwoInputEvent;

        var myActivityTwo = await context.CallActivityAsync<string>(
            Constants.MyActivityTwo, myActivityTwoInputEvent);

        myOrchestrationDto.MyActivityTwoResult = myActivityTwo;

        if (!context.IsReplaying)
        {
            _logger.LogWarning("myActivityTwo completed {myActivityTwo}", myActivityTwo);
        }

        return myOrchestrationDto;
    }
}