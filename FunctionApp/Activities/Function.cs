using System.IO;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Octokit;

namespace FunctionApp.Activities
{
    public class CreateIssueActivity
    {
        private readonly IGitHubClient _github;

        public CreateIssueActivity(IGitHubClient github) => _github = github;

        [FunctionName(nameof(CreateIssueActivity))]
        public void Run([ActivityTrigger]PostData input)
        {
            _github.Issue.Create(1234L, new NewIssue(input.Title) {Body = input.Description});
        }
    }
}