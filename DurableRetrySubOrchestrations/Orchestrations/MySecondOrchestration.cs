using DurableRetrySubOrchestrations.Model;
using Microsoft.Azure.Functions.Worker;
using Microsoft.DurableTask;
using Microsoft.Extensions.Logging;

namespace DurableRetrySubOrchestrations.Orchestrations;

public class MySecondOrchestration
{
    private readonly ILogger<MySecondOrchestration> _logger;

    public MySecondOrchestration(ILogger<MySecondOrchestration> logger)
    {
        _logger = logger;
    }

    [Function(Constants.MySecondOrchestration)]
    public async Task<MySubOrchestrationDto> RunOrchestrator(
        [OrchestrationTrigger] TaskOrchestrationContext context)
    {
        var mySubOrchestrationDto = new MySubOrchestrationDto
        {
            InputStartData = context.GetInput<string>()
        };

        if (!context.IsReplaying)
        {
            _logger.LogWarning("begin MySecondOrchestration with input {InputString}", context.GetInput<string>());
        }

        var retryOptions = new TaskOptions(
            new RetryPolicy(
                maxNumberOfAttempts: 5,
                firstRetryInterval: TimeSpan.FromSeconds(3),
                backoffCoefficient: 1.5));

        var myActivityThreeResult = await context.CallActivityAsync<string>
            (Constants.MyActivityThree, context.GetInput<string>(), retryOptions);

        mySubOrchestrationDto.MyActivityThreeResult = myActivityThreeResult;

        if (!context.IsReplaying)
        {
            _logger.LogWarning("MySecondOrchestration MyActivityThree completed {myActivityThreeResult}", myActivityThreeResult);
        }

        var myActivityFourResult = await context.CallActivityAsync<string>(
            Constants.MyActivityFour, "MySecondOrchestration second Activity");

        mySubOrchestrationDto.MyActivityFourResult = myActivityFourResult;

        if (!context.IsReplaying)
        {
            _logger.LogWarning("MySecondOrchestration MyActivityFour completed {myActivityFourResult}", myActivityFourResult);
        }

        return mySubOrchestrationDto;
    }
}