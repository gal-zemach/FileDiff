using FileDiff.Diff.Algorithm;

namespace FileDiff.Tests;

public class MyersComparerTests
{
    public static IEnumerable<TestCaseData> EqualTestData
    {
        get
        {
            yield return new TestCaseData(TestData.Empty, TestData.Empty);
            yield return new TestCaseData(TestData.Single, TestData.Single);
            yield return new TestCaseData(TestData.Simple4Elements, TestData.Simple4Elements);
        }
    }
    
    public static IEnumerable<TestCaseData> OneElementTestData
    {
        get
        {
            yield return new TestCaseData(TestData.Single, TestData.Empty);
            yield return new TestCaseData(TestData.Empty, TestData.Single);
            yield return new TestCaseData(TestData.Simple3Elements, TestData.Simple4Elements);
            yield return new TestCaseData(TestData.Simple4Elements, TestData.Simple3Elements);
        }
    }
    
    [TestCaseSource(nameof(EqualTestData))]
    public void Compare_EqualLists_ReturnsEmpty(List<string> lines1, List<string> lines2)
    {
        var result = MyersComparer.Compare(lines1, lines2);
        
        Assert.IsEmpty(result);
    }
    
    [TestCaseSource(nameof(OneElementTestData))]
    public void Compare_ListsWithExtraElement_SingleInstruction(List<string> lines1, List<string> lines2)
    {
        var result = MyersComparer.Compare(lines1, lines2);
        
        Assert.IsTrue(result.Count == 1);
    }
}

public static class TestData
{
    public static readonly List<string> Empty = [];
    public static readonly List<string> Single = ["a"];
    public static readonly List<string> Simple3Elements = ["a", "b", "c"];
    public static readonly List<string> Simple4Elements = ["a", "b", "c", "d"];
}
