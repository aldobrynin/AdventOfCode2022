namespace AoC2019.Day07;

public static partial class Day07 {
    public static void Solve(IEnumerable<string> input) {
        var program = input.Single().ToLongArray();
        new[] { 0, 1, 2, 3, 4 }.Permutations()
            .Max(p => p.Aggregate(0L, (acc, phase) => new IntCodeComputer(program, [phase, acc]).GetNextOutput().GetAwaiter().GetResult()))
            .Part1();

        new[] { 5, 6, 7, 8, 9 }.Permutations().Max(p => Calculate(program, p)).Part2();
    }

    private static long Calculate(long[] program, int[] phases) {
        var computers = phases.Select((p, i) => new IntCodeComputer(program, i == 0 ? [p, 0] : [p])).ToArray();
        foreach (var (first, second) in computers.Zip(computers.Skip(1).Append(computers[0]))) {
            first.PipeOutputTo(second.AddInput);
        }

        while (computers.All(c => c.RunToNextOutput().GetAwaiter().GetResult())) {
        }

        return computers[^1].Output;
    }
}