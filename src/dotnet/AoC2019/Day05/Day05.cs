namespace AoC2019.Day05;

public static partial class Day05 {
    public static void Solve(IEnumerable<string> input) {
        var instructions = input.Single().ToLongArray();
        new IntCodeComputer(instructions, [1]).RunToEnd().GetAwaiter().GetResult().Part1();
        new IntCodeComputer(instructions, [5]).RunToEnd().GetAwaiter().GetResult().Part2();
    }
}