using DurableWait.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.DurableTask.Client;

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
            HttpRequest request,
            DurableTaskClient client)
        {
            await client.ScheduleNewOrchestrationInstanceAsync(Constants.MyOrchestration, beginRequestData, new StartOrchestrationOptions { InstanceId = beginRequestData.Id });
            _log.LogInformation($"Started orchestration with ID = '{beginRequestData.Id}'.");

            TimeSpan timeout = TimeSpan.FromSeconds(30);

            var data = await client.WaitForInstanceCompletionAsync(beginRequestData.Id, timeout);

            // timeout
            if(data == null || !data.RuntimeStatus.IsCompletedOrTerminated())
            {
                await client.TerminateInstanceAsync(beginRequestData.Id, "Timeout something took too long");
                return new ContentResult()
                {
                    Content = "{ error: \"Timeout something took too long\" }",
                    ContentType = "application/json",
                    StatusCode = (int)HttpStatusCode.InternalServerError
                };
            }
            var output = data.ReadOutputAs<MyOrchestrationDto>();

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
