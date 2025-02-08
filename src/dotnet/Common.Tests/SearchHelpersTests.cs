namespace Common.Tests;

public class SearchHelpersTests {
    [Test]
    public void BinarySearch_LowerBound_Test1() {
        var array = new[] { 1, 2, 3, 3, 3, 4, 5, 10, 124 };

        var result = SearchHelpers.BinarySearchLowerBound(0, array.Length, i => array[i] >= 3);

        result.Should().Be(2);
    }

    [Test]
    public void BinarySearch_LowerBound_Test2() {
        var array = new[] { 1, 2, 3, 3, 3, 4, 5, 10, 124 };

        var result = SearchHelpers.BinarySearchLowerBound(0, array.Length, i => array[i] >= 4);

        result.Should().Be(5);
    }

    [Test]
    public void BinarySearch_UpperBound_Test1() {
        var array = new[] { 1, 2, 3, 3, 3, 4, 5, 10, 124 };

        var result = SearchHelpers.BinarySearchUpperBound(0, array.Length, i => array[i] < 3);

        result.Should().Be(1);
    }

    [Test]
    public void BinarySearch_UpperBound_Test2() {
        var array = new[] { 1, 2, 3, 3, 3, 4, 5, 10, 124 };

        var result = SearchHelpers.BinarySearchUpperBound(0, array.Length, i => array[i] <= 3);

        result.Should().Be(4);
    }
}
