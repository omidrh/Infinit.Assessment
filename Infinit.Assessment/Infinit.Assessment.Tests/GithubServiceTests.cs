using Infinit.Assessment.Services.Dtos.StatsDtos;
using Infinit.Assessment.Services.Implementations;
using Microsoft.Extensions.Configuration;
using Moq;
using Moq.Protected;
using System.Net;
using System.Text.Json;

namespace Infinit.Assessment.Tests;

[TestFixture]
public class GithubServiceTests
{
    private Mock<HttpMessageHandler> httpMessageHandlerMock;
    private GithubService service;

    [SetUp]
    public void Setup()
    {
        httpMessageHandlerMock = new Mock<HttpMessageHandler>();
        var httpClient = new HttpClient(httpMessageHandlerMock.Object);

        var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(
                [
                    new("GitHub:PersonalAccessToken", "test_token")
                ])
                .Build();

        service = new GithubService(httpClient, configuration);
    }

    [Test]
    public async Task GetJavaScriptFilesAsync_ReturnsFilteredFiles()
    {
        // Arrange
        var jsonResponse = new
        {
            tree = new[]
            {
                new { path = "file1.js", type = "blob" },
                new { path = "file2.ts", type = "blob" },
                new { path = "folder", type = "tree" }
            }
        };

        var response = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(JsonSerializer.Serialize(jsonResponse))
        };

        httpMessageHandlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(response);

        // Act
        HashSet<GithubFileNodeDto> result = await service.GetJavaScriptFilesAsync("lodash", "lodash", "main", CancellationToken.None);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.Count, Is.EqualTo(2));
            Assert.That(result.Any(file => file.Path == "file1.js" && file.Type == "blob"));
            Assert.That(result.Any(file => file.Path == "file2.ts" && file.Type == "blob"));
        });
    }

}
