using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.AutoNSubstitute;
using AzureFunctions.TestHelpers;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NSubstitute;
using Octokit;
using Xunit;

namespace FunctionApp.IntegrationTests
{
    public class UnitTest1
    {
        [Fact]
        public async Task Test1()
        {
            var fixture = new Fixture().Customize(new AutoNSubstituteCustomization {ConfigureMembers = true});
            var issue = fixture.Freeze<IIssuesClient>();
            
            var host = new HostBuilder()
                .ConfigureWebJobs(builder => builder.AddHttp())
                .ConfigureServices(services => services.AddSingleton(fixture.Create<IGitHubClient>()))
                .Build();

            await host.StartAsync();
            var jobs = host.Services.GetService<IJobHost>();


            using var content = new StringContent("{ \"title\": \"some new title\" }");
            var request = new DummyHttpRequest { Body = await content.ReadAsStreamAsync() };
            request.Headers.Add("Content-Type", "application/json");
            
            await jobs.CallAsync(nameof(Function), new Dictionary<string, object>
            {
                ["request"] = request
            });

            await issue.Received().Create(1234L, Arg.Any<NewIssue>());
        }
    }
}