using System;
using System.IO;
using FluentAssertions;
using Xunit;

namespace FunctionApp.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void ParsePullRequestCreated()
        {
            var message = Id(File.ReadAllText(Path.Combine("Messages", "PRCreated.json")));
            message.Should().BeEquivalentTo(new PullRequestMessage { Id = 69828, Status = "active"});
        }

        private static PullRequestMessage Id(string message)
        {
            var document = System.Text.Json.JsonDocument.Parse(message);
            var resource = document.RootElement.GetProperty("resource");

            return new PullRequestMessage
            {
                Id = resource.GetProperty("pullRequestId").GetInt32(),
                Status = resource.GetProperty("status").GetString()
            };
        }
    }
}