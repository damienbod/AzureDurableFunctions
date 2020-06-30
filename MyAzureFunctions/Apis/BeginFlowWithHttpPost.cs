using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using System.Net.Http;

namespace MyAzureFunctions.Apis
{
    public class BeginFlowWithHttpPost
    {
        [FunctionName(Constants.BeginFlowWithHttpPost)]
        public async Task<HttpResponseMessage> HttpStart(
          [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestMessage req,
          [DurableClient] IDurableOrchestrationClient starter,
          ILogger log)
        {
            string instanceId = await starter.StartNewAsync(Constants.MyOrchestration, null, "input data to start flow");
            log.LogInformation($"Started orchestration with ID = '{instanceId}'.");

            return starter.CreateCheckStatusResponse(req, instanceId);
        }
    }
}
