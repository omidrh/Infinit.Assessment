using Infinit.Assessment.Services.Contracts;
using Infinit.Assessment.Services.Dtos.StatsDtos;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;

namespace Infinit.Assessment.Services.Implementations;

public class GithubService : IGithubService
{
    private readonly HttpClient httpClient;

    public GithubService(
        HttpClient httpClient, 
        IConfiguration configuration)
    {
        this.httpClient = httpClient;
        this.httpClient.DefaultRequestHeaders.Add("User-Agent", "Infinit.Assessment - Omid");
        this.httpClient.BaseAddress = new Uri("https://api.github.com/");

        string token = configuration["GitHub:PersonalAccessToken"]
            ?? throw new InvalidOperationException("GitHub Personal Access Token is not configured.");
        this.httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }

    public async Task<HashSet<GithubFileNodeDto>> GetJavaScriptFilesAsync(string repositoryOwner, string repositoryName, string branch, CancellationToken cancellationToken)
    {
        IList<GithubFileNodeDto> files = await GetRepositoryTreeAsync(repositoryOwner, repositoryName, branch, cancellationToken);

        // NOTE: for extensibility we can retrieve the file types from outside
        List<string> fileTypes = ["js", "ts"];

        return files
            .Where(p => p.Type == "blob")
            .Where(p => fileTypes.Any(ext => p.Path.EndsWith($".{ext}")))
            .ToHashSet();
    }

    public async Task<string[]> GetFileContentsAsync(HashSet<GithubFileNodeDto> fileNodeDtos, CancellationToken cancellationToken)
    {
        List<Task<string>> fileContentTasks = fileNodeDtos
            .Select(file => GetFileContentAsync(file.Url, cancellationToken))
            .ToList();

        return await Task.WhenAll(fileContentTasks);
    }

    public async Task<string> GetFileContentAsync(string url, CancellationToken cancellationToken)
    {
        GithubFileContentDto githubFileContent = await httpClient.GetFromJsonAsync<GithubFileContentDto>(url, cancellationToken)
            ?? throw new HttpRequestException($"Failed to get the file content: {url}.");

        byte[] decodedBytes = Convert.FromBase64String(githubFileContent.Content);
        return Encoding.UTF8.GetString(decodedBytes);
    }

    private async Task<IList<GithubFileNodeDto>> GetRepositoryTreeAsync(string repositoryOwner, string repositoryName, string branch, CancellationToken cancellationToken)
    {
        string apiUrl = $"/repos/{repositoryOwner}/{repositoryName}/git/trees/{branch}?recursive=1";

        GithubTreeResponse? githubResponse = await httpClient.GetFromJsonAsync<GithubTreeResponse>(apiUrl, cancellationToken);

        return githubResponse?.Tree
            ?? throw new HttpRequestException("Failed to fetch repository tree.");
    }
}
