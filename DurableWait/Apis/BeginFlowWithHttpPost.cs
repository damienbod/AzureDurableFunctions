using DurableWait.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.DurableTask.Client;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace DurableWait.Apis;

public class BeginFlowWithHttpPost
{
    private readonly Processing _processing;
    private readonly ILogger<BeginFlowWithHttpPost> _logger;

    public BeginFlowWithHttpPost(ILogger<BeginFlowWithHttpPost> logger, Processing processing)
    {
        _logger = logger;
        _processing = processing;
    }

    [Function(Constants.BeginFlowWithHttpPost)]
    public async Task<IActionResult> HttpStart(
      [HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequest request,
      [DurableClient] DurableTaskClient client)
    {
        _logger.LogInformation("Started new flow");

        var beginRequestData = await JsonSerializer.DeserializeAsync<BeginRequestData>(request.Body);
        _logger.LogInformation("Started new flow with ID = '{beginRequestDataId}'.", beginRequestData.Id);

        return await _processing.ProcessFlow(beginRequestData, request, client);
    }
}
