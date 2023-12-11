namespace AoC2020.Day06;

public partial class Day06
{
    public static void Solve(IEnumerable<string> input)
    {
        var groups = input.StringJoin(Environment.NewLine)
            .Split(Environment.NewLine + Environment.NewLine)
            .Select(line => line.Split(Environment.NewLine))
            .ToArray();
        groups
            .Select(line => line.SelectMany(c => c.ToCharArray()).Distinct().Count())
            .Sum()
            .Part1();

        groups
            .Select(line =>
                line.Skip(1).Aggregate(line.First().ToCharArray(), (cur, n) => cur.Intersect(n).ToArray()).Length)
            .Sum()
            .Part2();
    }
}