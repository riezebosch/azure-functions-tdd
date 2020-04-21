using System.Net.Http;
using System.Threading.Tasks;
using FunctionApp.Activities;
using Microsoft.AspNetCore.Http;
using NSubstitute;
using Octokit;
using Xunit;

namespace FunctionApp.Tests.Activities
{
    public class CreateIssueTest
    {
        [Fact]
        public async Task ShouldCreateNewIssue()
        {
            // Arrange 
            var data = new PostData {Title = "some title", Description = "some description"};

            var issues = Substitute.For<IIssuesClient>();
            var github = Substitute.For<IGitHubClient>();
            github.Issue.Returns(issues);
            
            // Act
            var function = new CreateIssueActivity(github);
            function.Run(data);

            // Assert
            await issues.Received().Create(1234L, Arg.Is<NewIssue>(x => x.Title == "some title" && x.Body == "some description"));
        }
    }
}