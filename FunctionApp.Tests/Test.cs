using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using NSubstitute;
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

            // Act
            var sr =  new StreamReader(request.Body);
            var data = JsonSerializer.Deserialize<PostData>(sr.ReadToEnd(), new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

            // Assert
            data
                .Should()
                .BeEquivalentTo(
                    new PostData
                    {
                        Title = "some title",
                        Description = "some description"
                    });
        }
    }

    public class PostData
    {
        public string Title { get; set; }
        public string Description { get; set; }
    }
}