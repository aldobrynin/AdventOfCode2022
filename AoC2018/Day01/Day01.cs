namespace AoC2018.Day01;

public static partial class Day01 {
    public static void Solve(IEnumerable<string> input) {
        var changes = input.SelectMany(x => x.ToIntArray()).ToArray();
        changes.Sum().Part1();

        var visited = new HashSet<int>();
        changes
            .Repeat(100_000)
            .RunningSum(x => x)
            .First(x => !visited.Add(x))
            .Part2();
    }
}
