using System.ComponentModel;

namespace Infinit.Assessment.Services.Dtos.StatsDtos;

public class AnalyzeRequest
{
    private const string DefaultBranch = "main";
    private const string DefaultRepositoryOwner = "lodash";
    private const string DefaultRepositoryName = "lodash";

    private string branch = DefaultBranch;

    [DefaultValue(DefaultRepositoryOwner)]
    public string RepositoryOwner { get; set; } = DefaultRepositoryOwner;

    [DefaultValue(DefaultRepositoryName)]
    public string RepositoryName { get; set; } = DefaultRepositoryName;

    [DefaultValue(DefaultBranch)]
    public string Branch
    {
        get => branch;
        set => branch = string.IsNullOrWhiteSpace(value) ? DefaultBranch : value;
    }

    // NOTE: we may need to add file types here
    //public List<string> FileTypes { get; set; } = new() { "js", "ts" };
}