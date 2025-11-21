using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc;
using DurableWait.Model;
using Microsoft.Azure.Functions.Worker;
using Microsoft.AspNetCore.Http;
using Microsoft.DurableTask.Client;

namespace DurableWait.Apis
{
    
    public class BeginFlowWithHttpPost
    {
        private readonly Processing _processing;

        public BeginFlowWithHttpPost(Processing processing)
        {
            _processing = processing;
        }

        [Function(Constants.BeginFlowWithHttpPost)]
        public async Task<IActionResult> HttpStart(
          [HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequest request,
          [DurableClient] DurableTaskClient client,
          ILogger log)
        {
            log.LogInformation("Started new flow");

            BeginRequestData beginRequestData = await request.Content.ReadAsAsync<BeginRequestData>();
            log.LogInformation($"Started new flow with ID = '{beginRequestData.Id}'.");

            return await _processing.ProcessFlow(beginRequestData, request, client);
        }
    }
}
