using FunctionApp.Activities;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;

namespace FunctionApp.Orchestrations
{
    public class Orchestration
    {
        [FunctionName(nameof(Orchestration))]
        public void Run([OrchestrationTrigger]IDurableOrchestrationContext context)
        {
            context.CallActivityAsync<object>(nameof(CreateIssueActivity), context.GetInput<PostData>());
        }
    }
}