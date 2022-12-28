using Common;

namespace Solution;

public record ElvesPair(Range First, Range Second)
{
    public static ElvesPair Parse(string input)
    {
        var ranges = input.Split(',');
        return new ElvesPair(Range.FromString(ranges[0]), Range.FromString(ranges[1]));
    }
}

public class Day4
{
    public static void Solve(IEnumerable<string> input)
    {
        var pairs = input.Select(ElvesPair.Parse).ToArray();
        pairs
            .Count(x => x.First.Contains(x.Second) || x.Second.Contains(x.First))
            .Dump("Part1: ");
        
        pairs
            .Count(x => x.First.Overlaps(x.Second) || x.Second.Overlaps(x.First))
            .Dump("Part2: ");
    }
}