using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;

namespace MyAzureFunctions.Apis
{
    public static class ExternalHttpPostInput
    {
        [FunctionName(Constants.ExternalHttpPostInput)]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            [DurableClient] IDurableOrchestrationClient client,
            ILogger log)
        {
            string instanceId = req.Query["instanceId"];
            var status = await client.GetStatusAsync(instanceId);
            await client.RaiseEventAsync(instanceId, Constants.MyActivityTwo, "inputDataTwo");
          
            log.LogInformation("C# HTTP trigger function processed a request.");

            string responseMessage = string.IsNullOrEmpty(instanceId)
                ? "This HTTP triggered function executed successfully. Pass an instanceId in the query string"
                : $"Received, processing, {instanceId}";

            return new OkObjectResult(responseMessage);
        }
    }
}
