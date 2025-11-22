using DurableRetrySubOrchestrations.Model;
using Microsoft.Azure.Functions.Worker;
using Microsoft.DurableTask;
using Microsoft.Extensions.Logging;

namespace DurableRetrySubOrchestrations.Orchestrations;

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
            _logger.LogWarning("begin MyOrchestration with input {InputStartData}", context.GetInput<string>());
        }

        var retryOptions = new TaskOptions(
            new RetryPolicy(
                maxNumberOfAttempts: 5,
                firstRetryInterval: TimeSpan.FromSeconds(3),
                backoffCoefficient: 1.5));

        var myActivityOne = await context.CallActivityAsync<string>
            (Constants.MyActivityOne, context.GetInput<string>(), retryOptions);

        myOrchestrationDto.MyActivityOneResult = myActivityOne;

        if (!context.IsReplaying)
        {
            _logger.LogWarning("myActivityOne completed {myActivityOne}", myActivityOne);
        }

        var mySubOrchestrationDto = await context.CallSubOrchestratorAsync<MySubOrchestrationDto>
           (Constants.MySecondOrchestration, myActivityOne, retryOptions);

        myOrchestrationDto.MySubOrchestration = mySubOrchestrationDto;

        if (!context.IsReplaying)
        {
            _logger.LogWarning("mySubOrchestrationDto completed {mySubOrchestrationDtoMyActivityThreeResult}", mySubOrchestrationDto.MyActivityThreeResult);
        }

        var myActivityTwo = await context.CallActivityAsync<string>(
            Constants.MyActivityTwo, "Start MyOrchestration Activity 2");

        myOrchestrationDto.MyActivityTwoResult = myActivityTwo;

        if (!context.IsReplaying)
        {
            _logger.LogWarning("myActivityTwo completed {myActivityTwo}", myActivityTwo);
        }

        return myOrchestrationDto;
    }
}