using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.DurableTask.Client;
using Microsoft.Extensions.Logging;

namespace DurableWait.Diagnostics;
public class DiagnosticsApi
{
    private readonly ILogger<DiagnosticsApi> _logger;

    public DiagnosticsApi(ILogger<DiagnosticsApi> logger)
    {
        _logger = logger;
    }

    [Function(Constants.Diagnostics)]
    public async Task<IActionResult> Diagnostics(
     [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
     [FromServices] DurableTaskClient starter)
    {
        string instanceId = req.Query["instanceId"];
        _logger.LogInformation("Started DiagnosticsApi with ID = '{instanceId}'.", instanceId);

        var data = await starter.GetInstanceAsync(instanceId, true);
        return new OkObjectResult(data);
    }

    //[FunctionName("Diagnostics2")]
    //public IActionResult DiagnosticsApiReq(
    // [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
    // [DurableClient] IDurableOrchestrationClient starter,
    // ILogger log)
    //{
    //    string instanceId = req.Query["instanceId"];
    //    _logger.LogInformation("Started Diagnostics SPI with ID = '{instanceId}'.", instanceId);

    //    return starter.CreateCheckStatusResponse(req, trackingId);
    //}

    [Function(Constants.GetCompletedFlows)]
    public async Task<IActionResult> GetCompletedFlows(
    [HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequest req,
    [FromServices] DurableTaskClient client)
    {
        var runtimeStatus = new List<OrchestrationRuntimeStatus> {
            OrchestrationRuntimeStatus.Completed
        };

        return await FindOrchestrations(req, client, runtimeStatus,
            DateTime.UtcNow.AddDays(GetDays(req)),
            DateTime.UtcNow, true);
    }

    [Function(Constants.GetNotCompletedFlows)]
    public async Task<IActionResult> GetNotCompletedFlows(
    [HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequest req,
    [FromServices] DurableTaskClient client)
    {
        var runtimeStatus = new List<OrchestrationRuntimeStatus> {
            OrchestrationRuntimeStatus.Suspended,
            OrchestrationRuntimeStatus.Failed,
            OrchestrationRuntimeStatus.Pending,
            OrchestrationRuntimeStatus.Terminated
        };

        return await FindOrchestrations(req, client, runtimeStatus,
            DateTime.UtcNow.AddDays(GetDays(req)),
            DateTime.UtcNow, true);
    }

    [Function(Constants.GetAllFlows)]
    public async Task<IActionResult> GetAllFlows(
    [HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequest req,
    [FromServices] DurableTaskClient client)
    {
        var runtimeStatus = new List<OrchestrationRuntimeStatus> {
            OrchestrationRuntimeStatus.Running,
            OrchestrationRuntimeStatus.Suspended,
            OrchestrationRuntimeStatus.Failed,
            OrchestrationRuntimeStatus.Pending,
            OrchestrationRuntimeStatus.Terminated,
            OrchestrationRuntimeStatus.Completed
        };

        return await FindOrchestrations(req, client, runtimeStatus,
            DateTime.UtcNow.AddDays(GetDays(req)),
            DateTime.UtcNow, true);
    }

    private async Task<IActionResult> FindOrchestrations(
        HttpRequest req,
        DurableTaskClient client,
        IEnumerable<OrchestrationRuntimeStatus> runtimeStatus,
        DateTime from,
        DateTime to,
        bool showInput = false)
    {
        var query = new OrchestrationQuery(
            CreatedFrom: from,
            CreatedTo: to,
            Statuses: runtimeStatus,
            FetchInputsAndOutputs: showInput
        );

        var instances = new List<OrchestrationMetadata>();
        await foreach (var page in client.GetAllInstancesAsync(query))
        {
            instances.Add(page);
        }

        return new OkObjectResult(instances);
    }

    private static int GetDays(HttpRequest req)
    {
        string daysString = req.Query["days"];
        if (!string.IsNullOrEmpty(daysString))
        {
            var ok = int.TryParse(daysString, out int days);
            if (!ok)
            {
                return -1;
            }
            return -days;
        }

        return -1;
    }
}