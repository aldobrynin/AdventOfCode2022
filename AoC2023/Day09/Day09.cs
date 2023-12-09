namespace AoC2023.Day09;

public class Day09 {
    public static void Solve(IEnumerable<string> input) {
        input.Select(x => x.ToLongArray())
            .Select(Extrapolate)
            .ToArray()
            .Dump("Part1: ", transform: x => x.Sum(s => s.Last))
            .Dump("Part2: ", transform: x => x.Sum(s => s.First));
    }

    private static (long First, long Last) Extrapolate(long[] history) => FindDiffs(history)
        .Reduce((a, b) => (b.First - a.First, b.Last + a.Last));

    private static IEnumerable<(long First, long Last)> FindDiffs(long[] sequence) {
        var edges = new Stack<(long First, long Last)>();
        while (sequence.Any(x => x != 0)) {
            edges.Push((sequence[0], sequence[^1]));
            sequence = sequence.Zip(sequence.Skip(1), (a, b) => b - a).ToArray();
        }

        return edges;
    }
}