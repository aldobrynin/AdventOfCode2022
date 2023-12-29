using AoC2019.Day07;

namespace AoC2019.Day05;

public static partial class Day05 {
    public static void Solve(IEnumerable<string> input) {
        var instructions = input.Single().ToLongArray();
        new IntCodeComputer(instructions, [1]).RunToEnd().Part1();
        new IntCodeComputer(instructions, [5]).RunToEnd().Part2();
    }
}