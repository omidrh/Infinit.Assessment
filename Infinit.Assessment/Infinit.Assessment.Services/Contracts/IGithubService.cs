using Infinit.Assessment.Services.Dtos.StatsDtos;

namespace Infinit.Assessment.Services.Contracts;

public interface IGithubService
{
    Task<HashSet<GithubFileNodeDto>> GetJavaScriptFilesAsync(string repositoryOwner, string repositoryName, string branch, CancellationToken cancellationToken);
    Task<string[]> GetFileContentsAsync(HashSet<GithubFileNodeDto> githubFileNodeDtos, CancellationToken cancellationToken);
    Task<string> GetFileContentAsync(string url, CancellationToken cancellationToken);
}