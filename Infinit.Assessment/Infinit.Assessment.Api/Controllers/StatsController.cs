using Microsoft.AspNetCore.Mvc;
using Infinit.Assessment.Services.Contracts;
using Infinit.Assessment.Services.Dtos.StatsDtos;

namespace Infinit.Assessment.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StatsController(
    ILogger<StatsController> logger,
    IGithubService githubService, 
    IAnalysisService analysisService
    ) : ControllerBase
{
    [HttpGet("filtered-files")]
    public async Task<IActionResult> GetLodashJsAndTsFiles(CancellationToken cancellationToken)
    {
        AnalyzeRequest request = new();
        HashSet<GithubFileNodeDto> files = await githubService.GetJavaScriptFilesAsync(request.RepositoryOwner, request.RepositoryName, request.Branch, cancellationToken);

        return Ok(files);
    }

    [HttpPost("analyze")]
    public async Task<IActionResult> Analyze([FromBody] AnalyzeRequest request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Starting analysis for repository {RepositoryOwner}/{RepositoryName} on branch: {Branch}", request.RepositoryOwner, request.RepositoryName, request.Branch);

        // NOTE: we should move Validation part to a separated module
        if (string.IsNullOrWhiteSpace(request.RepositoryOwner) || string.IsNullOrWhiteSpace(request.RepositoryName))
        {
            return BadRequest("Repository owner and name are required.");
        }

        HashSet<GithubFileNodeDto> fileNodes = await githubService.GetJavaScriptFilesAsync(request.RepositoryOwner, request.RepositoryName, request.Branch, cancellationToken);

        string[] fileContents = await githubService.GetFileContentsAsync(fileNodes, cancellationToken);

        Dictionary<char, int> result = analysisService.AnalyzeLetterFrequency(fileContents);

        logger.LogInformation("Analysis completed successfully.");

        return Ok(result);
    }
}
