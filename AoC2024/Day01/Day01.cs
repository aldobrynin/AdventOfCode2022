namespace AoC2024.Day01;

public partial class Day01 {
    public static void Solve(IEnumerable<string> input) {
        var lines = input
            .Select(s => s.ToIntArray())
            .ToArray();
        var left = lines.Select(x => x[0]).Order().ToArray();
        var right = lines.Select(x => x[1]).Order().ToArray();

        left.Zip(right, (l, r) => Math.Abs(l - r))
            .Sum()
            .Part1();

    }

}
