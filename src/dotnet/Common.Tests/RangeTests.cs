using FluentAssertions.Formatting;

namespace Common.Tests;

public class RangeTests {
    [OneTimeSetUp]
    public void OneTimeSetup() {
        Formatter.AddFormatter(new RangeValueFormatter());
    }

    [Test]
    public void Merge_ShouldMergeOverlappingRanges() {
        new[] {
                Range.FromStartAndEnd(0, 11),
                Range.FromStartAndEnd(10, 20),
            }
            .Merge()
            .Should().BeEquivalentTo(new[] { Range.FromStartAndEnd(0, 20) });
    }

    [Test]
    public void Merge_ShouldNotMergeNonIntersectingRanges() {
        new[] {
                Range.FromStartAndEnd(0, 10),
                Range.FromStartAndEnd(11, 20),
            }
            .Merge()
            .Should().BeEquivalentTo(new[] {
                Range.FromStartAndEnd(0, 10),
                Range.FromStartAndEnd(11, 20),
            });
    }

    [Test]
    public void Merge_ShouldMergeAdjacentRanges() {
        new[] {
                Range.FromStartAndEnd(0, 10),
                Range.FromStartAndEnd(10, 20),
                Range.FromStartAndEnd(20, 30),
            }.Merge()
            .Should().BeEquivalentTo(new[] {
                Range.FromStartAndEnd(0, 30),
            });
    }

    [Test]
    public void Subtract_ShouldRemoveInnerRange() {
        Range.FromStartAndEnd(0, 10)
            .Subtract(Range.FromStartAndEnd(4, 5))
            .Should()
            .Be((Range.FromStartAndEnd(0, 4), Range.FromStartAndEnd(5, 10)));
    }

    [Test]
    public void Subtract_ShouldRemoveLeftRange() {
        Range.FromStartAndEnd(0, 10)
            .Subtract(Range.FromStartAndEnd(0, 5))
            .Should()
            .Be((null, Range.FromStartAndEnd(5, 10)));
    }

    [Test]
    public void Subtract_ShouldRemoveRightRange() {
        Range.FromStartAndEnd(0, 10)
            .Subtract(Range.FromStartAndEnd(5, 10))
            .Should()
            .Be((Range.FromStartAndEnd(0, 5), null));
    }

    [Test]
    public void Subtract_ShouldRemoveBothRanges() {
        Range.FromStartAndEnd(2, 10)
            .Subtract(Range.FromStartAndEnd(0, 10))
            .Should()
            .Be((null, null));
    }

    [Test]
    public void Subtract_ShouldRemoveNothing_WhenOtherRangeToTheLeft() {
        Range.FromStartAndEnd(0, 10)
            .Subtract(Range.FromStartAndEnd(-10, 0))
            .Should()
            .Be((null, Range.FromStartAndEnd(0, 10)));
    }

    [Test]
    public void Subtract_ShouldRemoveNothing_WhenOtherRangeToTheRight() {
        Range.FromStartAndEnd(0, 10)
            .Subtract(Range.FromStartAndEnd(20, 30))
            .Should()
            .Be((Range.FromStartAndEnd(0, 10), null));
    }

    [Test]
    public void Enumerate_ShouldReturnAllElementsInRange() {
        Range.FromStartAndEnd(0, 5)
            .Should().BeEquivalentTo(Enumerable.Range(0, 5));
    }

    private class RangeValueFormatter : IValueFormatter {
        public bool CanHandle(object? value) {
            if (value is null) return false;
            var type = value.GetType();
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Range<>);
        }

        public void Format(object? value, FormattedObjectGraph formattedGraph, FormattingContext context,
            FormatChild formatChild) {
            formattedGraph.AddFragment(value?.ToString());
        }
    }
}