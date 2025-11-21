using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.DurableTask.Client;

namespace DurableRetrySubOrchestrations.Diagnostics
{
    public class DiagnosticsApi
    {
        [Function(Constants.Diagnostics)]
        public async Task<IActionResult> Diagnostics(
         [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
         [FromServices] Microsoft.DurableTask.Client.DurableTaskClient starter,
         ILogger log)
        {
            string instanceId = req.Query["instanceId"];
            log.LogInformation($"Started DiagnosticsApi with ID = '{instanceId}'.");

            var data = await starter.GetStatusAsync(instanceId, true);
            return new OkObjectResult(data);
        }

        //[FunctionName("Diagnostics2")]
        //public IActionResult DiagnosticsApiReq(
        // [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
        // [DurableClient] IDurableOrchestrationClient starter,
        // ILogger log)
        //{
        //    string instanceId = req.Query["instanceId"];
        //    log.LogInformation($"Started Diagnostics SPI with ID = '{instanceId}'.");

        //    return starter.CreateCheckStatusResponse(req, trackingId);
        //}

        [Function(Constants.GetCompletedFlows)]
        public async Task<IActionResult> GetCompletedFlows(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequest req,
        [FromServices] Microsoft.DurableTask.Client.DurableTaskClient client,
        ILogger log)
        {
            var runtimeStatus = new List<Microsoft.DurableTask.Client.OrchestrationRuntimeStatus> {
                Microsoft.DurableTask.Client.OrchestrationRuntimeStatus.Completed
            };

            return await FindOrchestrations(req, client, runtimeStatus,
                DateTime.UtcNow.AddDays(GetDays(req)),
                DateTime.UtcNow, true);
        }

        [Function(Constants.GetNotCompletedFlows)]
        public async Task<IActionResult> GetNotCompletedFlows(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequest req,
        [FromServices] Microsoft.DurableTask.Client.DurableTaskClient client,
        ILogger log)
        {
            var runtimeStatus = new List<Microsoft.DurableTask.Client.OrchestrationRuntimeStatus> {
                Microsoft.DurableTask.Client.OrchestrationRuntimeStatus.Canceled,
                Microsoft.DurableTask.Client.OrchestrationRuntimeStatus.ContinuedAsNew,
                Microsoft.DurableTask.Client.OrchestrationRuntimeStatus.Failed,
                Microsoft.DurableTask.Client.OrchestrationRuntimeStatus.Pending,
                Microsoft.DurableTask.Client.OrchestrationRuntimeStatus.Terminated
            };

            return await FindOrchestrations(req, client, runtimeStatus,
                DateTime.UtcNow.AddDays(GetDays(req)),
                DateTime.UtcNow, true);
        }

        [Function(Constants.GetAllFlows)]
        public async Task<IActionResult> GetAllFlows(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequest req,
        [FromServices] Microsoft.DurableTask.Client.DurableTaskClient client,
        ILogger log)
        {
            var runtimeStatus = new List<Microsoft.DurableTask.Client.OrchestrationRuntimeStatus> {
                Microsoft.DurableTask.Client.OrchestrationRuntimeStatus.Running,
                Microsoft.DurableTask.Client.OrchestrationRuntimeStatus.Canceled,
                Microsoft.DurableTask.Client.OrchestrationRuntimeStatus.ContinuedAsNew,
                Microsoft.DurableTask.Client.OrchestrationRuntimeStatus.Failed,
                Microsoft.DurableTask.Client.OrchestrationRuntimeStatus.Pending,
                Microsoft.DurableTask.Client.OrchestrationRuntimeStatus.Terminated,
                Microsoft.DurableTask.Client.OrchestrationRuntimeStatus.Completed
            };

            return await FindOrchestrations(req, client, runtimeStatus,
                DateTime.UtcNow.AddDays(GetDays(req)),
                DateTime.UtcNow, true);
        }

        private async Task<IActionResult> FindOrchestrations(
            HttpRequest req,  
            Microsoft.DurableTask.Client.DurableTaskClient client,
            IEnumerable<OrchestrationRuntimeStatus> runtimeStatus,
            DateTime from,
            DateTime to,
            bool showInput = false)
        {
            // Define the cancellation token.
            CancellationTokenSource source = new CancellationTokenSource();
            CancellationToken token = source.Token;

            var instances = await client.ListInstancesAsync(
                new OrchestrationStatusQueryCondition
                {
                    CreatedTimeFrom = from,
                    CreatedTimeTo = to,
                    RuntimeStatus = runtimeStatus,
                    ShowInput = showInput
                },
                token
            );

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
}