using Infinit.Assessment.Services.Contracts;

namespace Infinit.Assessment.Services.Implementations;

public class AnalysisService : IAnalysisService
{
    public Dictionary<char, int> AnalyzeLetterFrequency(IEnumerable<string> fileContents)
    {
        Dictionary<char, int> letterCounts = [];

        // NOTE: if we want ot only consider English alphabets, we can initialize the dictionary with all of them
        //       to improve the performance removing the ContainsKey condition in foreach
        //Dictionary<char, int> letterCounts = Enumerable
        //        .Range('a', 26)
        //        .ToDictionary(c => (char)c, c => 0);

        foreach (string content in fileContents)
        {
            foreach (char c in content.ToLower().Where(char.IsLetter))
            {
                if (!letterCounts.ContainsKey(c))
                    letterCounts[c] = 0;

                letterCounts[c]++;
            }
        }

        return letterCounts
            .OrderByDescending(kv => kv.Value)
            .ToDictionary(kv => kv.Key, kv => kv.Value);
    }
}

