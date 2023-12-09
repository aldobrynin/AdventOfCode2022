namespace AoC2023.Day09;

public class Day09 {
    public static void Solve(IEnumerable<string> input) {
        var sequences = input.Select(x => x.ToLongArray()).ToArray();

        sequences.Sum(GetNext).Dump("Part1: ");

        sequences.Sum(GetPrev).Dump("Part2: ");
    }

    private static long GetNext(long[] sequence) {
        if (sequence.All(x => x == 0)) return 0;
        return sequence.Last() + GetNext(GetDifferences(sequence));
    }

    private static long GetPrev(long[] sequence) {
        if (sequence.All(x => x == 0)) return 0;
        return sequence.First() - GetPrev(GetDifferences(sequence));
    }

    private static long[] GetDifferences(long[] sequence) {
        return sequence.Zip(sequence.Skip(1), (a, b) => b - a).ToArray();
    }
}