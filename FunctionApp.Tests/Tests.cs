using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using NSubstitute;
using Octokit;
using Xunit;

namespace FunctionApp.Tests
{
    public class Tests
    {
        [Fact]
        public async Task Test1()
        {
            // Arrange 
            using var content = new StringContent("{ \"title\": \"some new issue\", \"description\": \"I like your software but you need to fix!\" }");
            var request = Substitute.For<HttpRequest>();
            request.Body = await content.ReadAsStreamAsync();

            var github = Substitute.For<IGitHubClient>();
            var issue = Substitute.For<IIssuesClient>();
            github.Issue.Returns(issue);

            // Act
            var function = new Function(github);
            await function.Run(request);
            
            // Assert
            await issue.Received().Create(Arg.Any<long>(), Arg.Is<NewIssue>(x => x.Title == "some new issue" && x.Body == "I like your software but you need to fix!"));
        }
    }
}