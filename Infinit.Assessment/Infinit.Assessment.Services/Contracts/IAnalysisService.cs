namespace Infinit.Assessment.Services.Contracts;

public interface IAnalysisService
{
    Dictionary<char, int> AnalyzeLetterFrequency(IEnumerable<string> fileContents);
}