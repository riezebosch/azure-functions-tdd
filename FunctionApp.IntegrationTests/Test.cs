using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.AutoNSubstitute;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NSubstitute;
using Octokit;
using Xunit;

namespace FunctionApp.IntegrationTests
{
    public class Test
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
                .ConfigureWebJobs(builder => builder.AddHttp())
                .ConfigureServices(services => services.AddSingleton(fixture.Create<IGitHubClient>()))
                .Build();

            await host.StartAsync();
            var jobs = host.Services.GetService<IJobHost>();

            await jobs.CallAsync(nameof(Function), new Dictionary<string, object>
            {
                ["request"] = request
            });

            await issues.Received().Create(1234L, Arg.Any<NewIssue>());
        }
    }
}