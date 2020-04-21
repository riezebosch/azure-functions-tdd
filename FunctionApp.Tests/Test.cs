using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using NSubstitute;
using Octokit;
using Xunit;

namespace FunctionApp.Tests
{
    public class Test
    {
        [Fact]
        public async Task Test1()
        {
            // Arrange 
            using var body = new StringContent("{ \"title\": \"some title\", \"description\": \"some description\" }");

            var request = Substitute.For<HttpRequest>();
            request.Body = await body.ReadAsStreamAsync();

            var issues = Substitute.For<IIssuesClient>();
            var github = Substitute.For<IGitHubClient>();
            github.Issue.Returns(issues);
            
            // Act
            var function = new Function(github);
            function.Run(request);

            // Assert
            await issues.Received().Create(1234L, Arg.Is<NewIssue>(x => x.Title == "some title" && x.Body == "some description"));
        }
    }
}