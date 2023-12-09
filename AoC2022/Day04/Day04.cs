using Range = Common.Range;

namespace Solution.Day04;

public record ElvesPair(Range<int> First, Range<int> Second) {
    public static ElvesPair Parse(string input) {
        var ranges = input.Split(',').Select(ToRange).ToArray();
        return new ElvesPair(ranges[0], ranges[1]);

        Range<int> ToRange(string s) {
            var parts = s.Split('-');
            return Range.FromStartAndEnd(parts[0].ToInt(), parts[1].ToInt());
        }
    }
}

public class Day4 {
    public static void Solve(IEnumerable<string> input) {
        var pairs = input.Select(ElvesPair.Parse).ToArray();
        pairs
            .Count(x => x.First.Contains(x.Second) || x.Second.Contains(x.First))
            .Dump("Part1: ");

        pairs
            .Count(x => x.First.Overlaps(x.Second))
            .Dump("Part2: ");
    }
}