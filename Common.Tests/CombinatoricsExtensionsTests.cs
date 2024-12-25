namespace Common.Tests;

public class CombinatoricsExtensionsTests {
    [TestCaseSource(nameof(VariantsTestCases))]
    public void VariantsTest(int[] values, int k, int[][] expected) {
        values.Variants(k).Should().BeEquivalentTo(expected);
    }

    [TestCaseSource(nameof(CombinationsTestCases))]
    public void CombinationsTest(int[] values, int k, int[][] expected) {
        values.Combinations(k).Should().BeEquivalentTo(expected);
    }

    [TestCaseSource(nameof(PermutationsTestCases))]
    public void PermutationsTest(int[] values, int[][] expected) {
        values.Permutations().Should().BeEquivalentTo(expected);
    }

    public static IEnumerable<TestCaseData> VariantsTestCases() {
        yield return new TestCaseData(new[] { 1, 2 }, 1, new int[][] { [1], [2] });
        yield return new TestCaseData(new[] { 1, 2 }, 2, new int[][] { [1, 1], [1, 2], [2, 1], [2, 2] });
        yield return new TestCaseData(new[] { 1, 2, 3 }, 2, new int[][] { [1, 1], [1, 2], [1, 3], [2, 1], [2, 2], [2, 3], [3, 1], [3, 2], [3, 3] });
    }

    public static IEnumerable<TestCaseData> CombinationsTestCases() {
        yield return new TestCaseData(new[] { 1, 2 }, 1, new int[][] { [1], [2] });
        yield return new TestCaseData(new[] { 1, 2 }, 2, new int[][] { [1, 2] });
        yield return new TestCaseData(new[] { 1, 2, 3 }, 2, new int[][] { [1, 2], [1, 3], [2, 3] });
        yield return new TestCaseData(new[] { 1, 2, 3 }, 3, new int[][] { [1, 2, 3] });
    }

    public static IEnumerable<TestCaseData> PermutationsTestCases() {
        yield return new TestCaseData(new[] { 1 }, new int[][] { [1] });
        yield return new TestCaseData(new[] { 1, 2 }, new int[][] { [1, 2], [2, 1] });
        yield return new TestCaseData(new[] { 1, 2, 3 }, new int[][] { [1, 2, 3], [1, 3, 2], [2, 1, 3], [2, 3, 1], [3, 1, 2], [3, 2, 1] });
    }
}
