using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs;
using Octokit;

namespace FunctionApp
{
    public class Function
    {
        private readonly IGitHubClient _github;

        public Function(IGitHubClient github) => _github = github;

        [FunctionName(nameof(Function))]
        public async Task Run([HttpTrigger]HttpRequest request)
        {
            var body = new StreamReader(request.Body).ReadToEnd();
            var data = JsonSerializer.Deserialize<PostData>(body, new JsonSerializerOptions {  PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
            
            await _github.Issue.Create(1234L, new NewIssue(data.Title) {Body = data.Description});
        }
    }
}