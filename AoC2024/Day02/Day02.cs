namespace AoC2024.Day02;

public static partial class Day02 {
    public static void Solve(IEnumerable<string> input) {
        var reports = input.Select(line => line.ToIntArray())
            .ToArray();

        reports.Count(IsSafe).Part1();
        reports.Count(IsSafe2).Part2();

        bool IsSafe(int[] report) {
            var diffs = report.ZipWithNext((a, b) => a - b).ToArray();
            return (diffs.All(x => x > 0) || diffs.All(x => x < 0)) &&
                   diffs.All(diff => Math.Abs(diff) is > 0 and <= 3);
        }

        bool IsSafe2(int[] report) =>
            report
                .Indices()
                .Any(indexToRemove => IsSafe([..report[..indexToRemove], ..report[(indexToRemove + 1)..]]));
    }
}
