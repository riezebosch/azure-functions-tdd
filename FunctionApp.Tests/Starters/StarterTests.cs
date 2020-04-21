using System.Net.Http;
using System.Threading.Tasks;
using FunctionApp.Orchestrations;
using FunctionApp.Starters;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using NSubstitute;
using Octokit;
using Xunit;

namespace FunctionApp.Tests.Starters
{
    public class StarterTests
    {
        [Fact]
        public async Task ShouldStartNewOrchestration()
        {
            // Arrange 
            using var body = new StringContent("{ \"title\": \"some title\", \"description\": \"some description\" }");

            var request = Substitute.For<HttpRequest>();
            request.Body = await body.ReadAsStreamAsync();

            var client = Substitute.For<IDurableOrchestrationClient>();
            
            // Act
            var function = new Starter();
            await function.Run(request, client);

            // Assert
            await client.Received().StartNewAsync(nameof(Orchestration), Arg.Is<PostData>(x => x.Title == "some title" && x.Description == "some description"));
        }
    }
}