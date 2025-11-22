using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.DurableTask.Client;
using Microsoft.Extensions.Logging;

namespace DurableRetrySubOrchestrations.Apis;

public class BeginFlowWithHttpPost
{
    private readonly ILogger<BeginFlowWithHttpPost> _logger;

    public BeginFlowWithHttpPost(ILogger<BeginFlowWithHttpPost> logger)
    {
        _logger = logger;
    }

    [Function(Constants.BeginFlowWithHttpPost)]
    public async Task<IActionResult> HttpStart(
      [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequest req,
      [DurableClient] DurableTaskClient starter)
    {
        string instanceId = await starter.ScheduleNewOrchestrationInstanceAsync(Constants.MyOrchestration, "input data to start flow");
        _logger.LogInformation("Started orchestration with ID = '{instanceId}'.", instanceId);

        return new OkObjectResult(new { instanceId });
    }
}
