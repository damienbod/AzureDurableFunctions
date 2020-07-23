using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;

namespace DurableWait.Diagnostics
{
    public class DiagnosticsApi
    {
        [FunctionName(Constants.DiagnosticsApi)]
        public IActionResult DiagnosticsApiReq(
         [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
         [DurableClient] IDurableOrchestrationClient starter,
         ILogger log)
        {
            string instanceId = req.Query["instanceId"];
            log.LogInformation($"Started DiagnosticsApi with ID = '{instanceId}'.");

            return starter.CreateCheckStatusResponse(req, instanceId);
        }
    }
}
