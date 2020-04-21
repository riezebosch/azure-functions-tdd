using AutoFixture;
using FunctionApp.Activities;
using FunctionApp.Orchestrations;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using NSubstitute;
using Xunit;

namespace FunctionApp.Tests.Orchestrations
{
    public class OrchestrationTests
    {
        [Fact]
        public void Test()
        {
            var fixture = new Fixture();
            var data = fixture.Create<PostData>();
            
            var context = Substitute.For<IDurableOrchestrationContext>();
            context.GetInput<PostData>().Returns(data);
            
            var function = new Orchestration();
            function.Run(context);

            context.Received().CallActivityAsync<object>(nameof(CreateIssueActivity), data);
        }
    }
}