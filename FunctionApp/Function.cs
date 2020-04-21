using System.IO;
using System.Text.Json;
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
        public void Run([HttpTrigger]HttpRequest request)
        {
            var sr = new StreamReader(request.Body);
            var data = JsonSerializer.Deserialize<PostData>(sr.ReadToEnd(),
                new JsonSerializerOptions {PropertyNamingPolicy = JsonNamingPolicy.CamelCase});

            _github.Issue.Create(1234L, new NewIssue(data.Title) {Body = data.Description});
        }
    }
}