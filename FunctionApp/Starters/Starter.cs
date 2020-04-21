using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using FunctionApp.Orchestrations;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Octokit;

namespace FunctionApp.Starters
{
    public class Starter
    {
        [FunctionName(nameof(Starter))]
        public async Task Run([HttpTrigger] HttpRequest request, [DurableClient]IDurableOrchestrationClient client)
        {
            var sr = new StreamReader(request.Body);
            var data = JsonSerializer.Deserialize<PostData>(sr.ReadToEnd(),
                new JsonSerializerOptions {PropertyNamingPolicy = JsonNamingPolicy.CamelCase});

            await client.StartNewAsync(nameof(Orchestration), data);
        }
    }
}