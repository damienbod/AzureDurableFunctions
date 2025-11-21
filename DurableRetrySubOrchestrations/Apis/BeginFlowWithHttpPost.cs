using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using Microsoft.Azure.Functions.Worker;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DurableTask.Client;

namespace DurableRetrySubOrchestrations.Apis
{
    public class BeginFlowWithHttpPost
    {
        [Function(Constants.BeginFlowWithHttpPost)]
        public async Task<IActionResult> HttpStart(
          [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequest req,
          [DurableClient] DurableTaskClient starter,
          ILogger log)
        {
            string instanceId = await starter.ScheduleNewOrchestrationInstanceAsync(Constants.MyOrchestration, "input data to start flow");
            log.LogInformation($"Started orchestration with ID = '{instanceId}'.");

            return new OkObjectResult(new { instanceId });
        }
    }
}
