using DurableWait.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace DurableWait
{
    public class Processing
    {
        private readonly ILogger<Processing> _log;

        public Processing(ILoggerFactory loggerFactory)
        {
            _log = loggerFactory.CreateLogger<Processing>();
        }

        public async Task<IActionResult> ProcessFlow(
            BeginRequestData beginRequestData, 
            HttpRequestMessage request,
            IDurableOrchestrationClient client)
        {
            await client.StartNewAsync(Constants.MyOrchestration, beginRequestData.Id, beginRequestData);
            _log.LogInformation($"Started orchestration with ID = '{beginRequestData.Id}'.");

            TimeSpan timeout = TimeSpan.FromSeconds(7);
            TimeSpan retryInterval = TimeSpan.FromSeconds(1);

            await client.WaitForCompletionOrCreateCheckStatusResponseAsync(
                request,
                beginRequestData.Id,
                timeout,
                retryInterval);

            var data = await client.GetStatusAsync(beginRequestData.Id);

            // timeout
            if(data.RuntimeStatus != OrchestrationRuntimeStatus.Completed)
            {
                await client.TerminateAsync(beginRequestData.Id, "Timeout something took too long");
                return new ContentResult()
                {
                    Content = "{ error: \"Timeout something took too long\" }",
                    ContentType = "application/json",
                    StatusCode = (int)HttpStatusCode.InternalServerError
                };
            }
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
