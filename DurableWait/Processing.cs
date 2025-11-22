using DurableWait.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DurableTask.Client;
using Microsoft.Extensions.Logging;
using System.Net;

namespace DurableWait;

public class Processing
{
    private readonly ILogger<Processing> _log;

    public Processing(ILoggerFactory loggerFactory)
    {
        _log = loggerFactory.CreateLogger<Processing>();
    }

    public async Task<IActionResult> ProcessFlow(
        BeginRequestData beginRequestData,
        HttpRequest request,
        DurableTaskClient client)
    {
        var instanceId = await client.ScheduleNewOrchestrationInstanceAsync(Constants.MyOrchestration, beginRequestData);
        _log.LogInformation("Started orchestration with ID = '{instanceId}'.", instanceId);

        // Create a timeout using CancellationTokenSource
        using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(30));

        OrchestrationMetadata data;
        try
        {
            data = await client.WaitForInstanceCompletionAsync(instanceId, getInputsAndOutputs: true, cts.Token);
        }
        catch (OperationCanceledException)
        {
            // Timeout occurred
            await client.TerminateInstanceAsync(instanceId, "Timeout something took too long");
            return new ContentResult()
            {
                Content = "{ \"error\": \"Timeout something took too long\" }",
                ContentType = "application/json",
                StatusCode = (int)HttpStatusCode.InternalServerError
            };
        }

        // Check if completed
        if (data == null || data.RuntimeStatus != OrchestrationRuntimeStatus.Completed)
        {
            await client.TerminateInstanceAsync(instanceId, "Timeout something took too long");
            return new ContentResult()
            {
                Content = "{ \"error\": \"Timeout something took too long\" }",
                ContentType = "application/json",
                StatusCode = (int)HttpStatusCode.InternalServerError
            };
        }
        var output = data.ReadOutputAs<MyOrchestrationDto>();

        var completeResponseData = new CompleteResponseData
        {
            BeginRequestData = output.BeginRequest,
            Id2 = output.BeginRequest.Id + ".v2",
            MyActivityTwoResult = output.MyActivityTwoResult
        };

        return new OkObjectResult(completeResponseData);
    }
}
