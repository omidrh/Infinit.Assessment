using Infinit.Assessment.Api.Controllers;
using Infinit.Assessment.Services.Contracts;
using Infinit.Assessment.Services.Dtos.StatsDtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace Infinit.Assessment.Tests;

[TestFixture]
public class StatsControllerTests
{
    private Mock<ILogger<StatsController>> loggerMock;
    private Mock<IGithubService> githubServiceMock;
    private Mock<IAnalysisService> analysisServiceMock;
    private StatsController controller;

    [SetUp]
    public void Setup()
    {
        loggerMock = new Mock<ILogger<StatsController>>();
        githubServiceMock = new Mock<IGithubService>();
        analysisServiceMock = new Mock<IAnalysisService>();
        controller = new StatsController(loggerMock.Object, githubServiceMock.Object, analysisServiceMock.Object);
    }

    [Test]
    public async Task GetLodashJsAndTsFiles_ReturnsFiles()
    {
        // Arrange
        HashSet<GithubFileNodeDto> expectedFiles = [new GithubFileNodeDto()];
        githubServiceMock
            .Setup(x => x.GetJavaScriptFilesAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedFiles);

        // Act
        IActionResult result = await controller.GetLodashJsAndTsFiles(CancellationToken.None);

        // Assert
        OkObjectResult? okResult = result as OkObjectResult;
        Assert.That(okResult, Is.Not.Null);
        Assert.That(okResult.Value, Is.EqualTo(expectedFiles));
    }

    [TestCase(null, "ValidRepoName")]
    [TestCase("", "ValidRepoName")]
    [TestCase("   ", "ValidRepoName")]
    [TestCase("ValidRepoOwner", null)]
    [TestCase("ValidRepoOwner", "")]
    [TestCase("ValidRepoOwner", "   ")]
    public async Task Analyze_InvalidRequest_ReturnsBadRequest(string? repositoryOwner, string? repositoryName)
    {
        // Arrange
        AnalyzeRequest request = new() { RepositoryOwner = repositoryOwner, RepositoryName = repositoryName };

        // Act
        IActionResult result = await controller.Analyze(request, CancellationToken.None);

        // Assert
        Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
    }

    [Test]
    public async Task Analyze_ValidRequest_ReturnsAnalysisResult()
    {
        // Arrange
        AnalyzeRequest request = new();
        Dictionary<char, int> letterFrequency = new() { { 'a', 4 }, { 'b', 3 }, { 'c', 2 } };

        githubServiceMock
            .Setup(x => x.GetJavaScriptFilesAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync([]);

        analysisServiceMock
            .Setup(x => x.AnalyzeLetterFrequency(It.IsAny<IEnumerable<string>>()))
            .Returns(letterFrequency);

        // Act
        IActionResult result = await controller.Analyze(request, CancellationToken.None);

        // Assert
        OkObjectResult? okResult = result as OkObjectResult;
        Assert.That(okResult, Is.Not.Null);
        Assert.That(okResult.Value, Is.EqualTo(letterFrequency));
    }
}
