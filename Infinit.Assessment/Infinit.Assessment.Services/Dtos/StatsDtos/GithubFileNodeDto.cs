namespace Infinit.Assessment.Services.Dtos.StatsDtos;

public class GithubFileNodeDto
{
    public string Path { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public int Size { get; set; }
    public string Url { get; set; } = string.Empty;
}
