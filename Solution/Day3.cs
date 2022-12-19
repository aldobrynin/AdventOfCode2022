using Common;

namespace Solution;

public class Day3
{
    public static void Solve(IEnumerable<string> input)
    {
        var lines = input.ToArray();
        lines
            .Select(x =>
            {
                var first = x[..(x.Length / 2)];
                var second = x.Substring(x.Length / 2, x.Length / 2);
                return first.Intersect(second).Single();
            })
            .Sum(ToScore)
            .Dump("Part1: ");
        
        lines
            .Chunk(3)
            .Select(x => x
                .Aggregate<IEnumerable<char>>((p, n) => p.Intersect(n))
                .Single())
            .Sum(ToScore)
            .Dump("Part2: ");
    }

    private static int ToScore(char c) => c >= 'a' ? c - 'a' + 1 : c - 'A' + 27;
}