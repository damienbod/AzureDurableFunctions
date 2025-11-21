using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.DurableTask.Client;
using Microsoft.Extensions.Logging;

namespace MyAzureFunctions.Apis;

public class ExternalHttpPostInput
{
    [Function(Constants.ExternalHttpPostInput)]
    public async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
        [DurableClient] DurableTaskClient client,

        ILogger log)
    {
        string instanceId = req.Query["instanceId"];
        var status = await client.GetInstanceAsync(instanceId);
        await client.RaiseEventAsync(instanceId, Constants.MyExternalInputEvent, "inputDataTwo");

        log.LogInformation("C# HTTP trigger function processed a request.");

        string responseMessage = string.IsNullOrEmpty(instanceId)
            ? "This HTTP triggered function executed successfully. Pass an instanceId in the query string"
            : $"Received, processing, {instanceId}";

        return new OkObjectResult(responseMessage);
    }
}
