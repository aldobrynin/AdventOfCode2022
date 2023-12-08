namespace Common.Tests;

public class RangeTests {
    [Test]
    public void Merge_ShouldMergeOverlappingRanges() {
        new[] {
                Range.FromStartAndEndInclusive(0, 11),
                Range.FromStartAndEndInclusive(10, 20),
            }
            .Merge()
            .Should().BeEquivalentTo(new[] { Range.FromStartAndEndInclusive(0, 20) });
    }

    [Test]
    public void Merge_ShouldNotMergeNonIntersectingRanges() {
        new[] {
                Range.FromStartAndEndInclusive(0, 10),
                Range.FromStartAndEndInclusive(11, 20),
            }
            .Merge()
            .Should().BeEquivalentTo(new[] {
                Range.FromStartAndEndInclusive(0, 10),
                Range.FromStartAndEndInclusive(11, 20),
            });
    }

    [Test]
    public void Merge_ShouldMergeAdjacentRanges() {
        new[] {
                Range.FromStartAndEndInclusive(0, 10),
                Range.FromStartAndEndInclusive(10, 20),
                Range.FromStartAndEndInclusive(20, 30),
            }.Merge()
            .Should().BeEquivalentTo(new[] {
                Range.FromStartAndEndInclusive(0, 30),
            });
    }

    [Test]
    public void Subtract_ShouldRemoveInnerRange() {
        Range.FromStartAndEndInclusive(0, 10)
            .Subtract(Range.FromStartAndEndInclusive(4, 5))
            .Should()
            .BeEquivalentTo(new[] {
                Range.FromStartAndEndInclusive(0, 3),
                Range.FromStartAndEndInclusive(6, 10),
            });
    }
}