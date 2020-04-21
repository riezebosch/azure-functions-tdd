using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.AutoNSubstitute;
using AzureFunctions.TestHelpers;
using FunctionApp.Orchestrations;
using FunctionApp.Starters;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NSubstitute;
using Octokit;
using Xunit;

namespace FunctionApp.IntegrationTests
{
    public class Durable
    {
        [Fact]
        public async Task Test2()
        {
            using var body = new StringContent("{ \"title\": \"some title\", \"description\": \"some description\" }");

            var request = Substitute.For<HttpRequest>();
            request.Body = await body.ReadAsStreamAsync();

            var fixture = new Fixture().Customize(new AutoNSubstituteCustomization {ConfigureMembers = true});
            var issues = fixture.Freeze<IIssuesClient>();
            
            using var host = new HostBuilder()
                .ConfigureWebJobs(builder => builder.AddHttp().AddDurableTask())
                .ConfigureServices(services => services.AddSingleton(fixture.Create<IGitHubClient>()))
                .Build();

            await host.StartAsync();
            var jobs = host.Services.GetService<IJobHost>();

            await jobs
                .Terminate()
                .Purge();
            
            await jobs.CallAsync(nameof(Starter), new Dictionary<string, object>
            {
                ["request"] = request
            });

            await jobs
                .WaitFor(nameof(Orchestration))
                .ThrowIfFailed();

            await issues.Received().Create(1234L, Arg.Any<NewIssue>());
        }
    }
}