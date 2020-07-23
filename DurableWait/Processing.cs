using DurableWait.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace DurableWait
{
    public class Processing
    {
        private readonly ILogger<Processing> _log;
        private readonly IDurableOrchestrationClient _client;

        public Processing(IDurableOrchestrationClient client, ILoggerFactory loggerFactory)
        {
            _log = loggerFactory.CreateLogger<Processing>();
            _client = client;
        }

        public async Task<IActionResult> RunAndReturnWithCompletedResult(
            BeginRequestData beginRequestData, 
            HttpRequestMessage request)
        {
            await _client.StartNewAsync(Constants.MyOrchestration, beginRequestData.Id, beginRequestData);
            _log.LogInformation($"Started orchestration with ID = '{beginRequestData.Id}'.");

            TimeSpan timeout = TimeSpan.FromSeconds(10);
            TimeSpan retryInterval = TimeSpan.FromSeconds(1);

            await _client.WaitForCompletionOrCreateCheckStatusResponseAsync(
                request,
                beginRequestData.Id,
                timeout,
                retryInterval);

            var data = await _client.GetStatusAsync(beginRequestData.Id);
            var output = data.Output.ToObject<MyOrchestrationDto>();

            var completeResponseData = new CompleteResponseData
            {
                BeginRequestData = output.BeginRequest,
                Id2 = output.BeginRequest.Id + ".v2",
                MyActivityTwoResult = output.MyActivityTwoResult
            };

            return new OkObjectResult(completeResponseData);
        }
    }
}
