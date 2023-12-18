namespace AoC2020.Day06;

public partial class Day06 {
    public static void Solve(IEnumerable<string> input) {
        var groups = input.SplitBy(string.IsNullOrWhiteSpace).ToArray();
        groups
            .Select(line => line.Flatten().Distinct().Count())
            .Sum()
            .Part1();

        groups
            .Select(line => line.IntersectAll().Count)
            .Sum()
            .Part2();
    }
}