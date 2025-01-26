using Infinit.Assessment.Services.Implementations;

namespace Infinit.Assessment.Tests;

[TestFixture]
public class AnalysisServiceTests
{
    private AnalysisService analysisService;

    [SetUp]
    public void Setup()
    {
        analysisService = new AnalysisService();
    }

    [Test]
    public void AnalyzeLetterFrequency_ReturnsCorrectCounts_ForSimpleInput()
    {
        // Arrange
        List<string> input = ["abc", "aaabbc"];

        // Act
        Dictionary<char, int> result = analysisService.AnalyzeLetterFrequency(input);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result['a'], Is.EqualTo(4));
            Assert.That(result['b'], Is.EqualTo(3));
            Assert.That(result['c'], Is.EqualTo(2));
        });
    }

    [Test]
    public void AnalyzeLetterFrequency_IsCaseInsensitive()
    {
        // Arrange
        List<string> input = ["AaBbBCccC"];

        // Act
        Dictionary<char, int> result = analysisService.AnalyzeLetterFrequency(input);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result['a'], Is.EqualTo(2));
            Assert.That(result['b'], Is.EqualTo(3));
            Assert.That(result['c'], Is.EqualTo(4));
        });
    }

    [Test]
    public void AnalyzeLetterFrequency_ExcludesNonLetterCharacters()
    {
        // Arrange
        List<string> input = ["a1b2c3!@#"];

        // Act
        Dictionary<char, int> result = analysisService.AnalyzeLetterFrequency(input);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result['a'], Is.EqualTo(1));
            Assert.That(result['b'], Is.EqualTo(1));
            Assert.That(result['c'], Is.EqualTo(1));
            Assert.That(result, Has.Count.EqualTo(3));
        });
    }

    [Test]
    public void AnalyzeLetterFrequency_ReturnsSortedResult()
    {
        // Arrange
        List<string> input = ["aaabbcccc"];

        // Act
        Dictionary<char, int> result = analysisService.AnalyzeLetterFrequency(input);

        // Assert
        char[] expectedOrder = ['c', 'a', 'b'];
        Assert.That(result.Keys, Is.EqualTo(expectedOrder));
    }

    [Test]
    public void AnalyzeLetterFrequency_ReturnsEmptyDictionary_ForEmptyInput()
    {
        // Arrange
        List<string> input = [];

        // Act
        Dictionary<char, int> result = analysisService.AnalyzeLetterFrequency(input);

        // Assert
        Assert.That(result, Is.Empty);
    }

    [Test]
    public void AnalyzeLetterFrequency_ReturnsEmptyDictionary_ForStringsWithoutLetters()
    {
        // Arrange
        List<string> input = ["12345", "!@#$%", "   "];

        // Act
        Dictionary<char, int> result = analysisService.AnalyzeLetterFrequency(input);

        // Assert
        Assert.That(result, Is.Empty);
    }
}
