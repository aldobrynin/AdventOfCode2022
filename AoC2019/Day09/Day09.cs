namespace AoC2019.Day09;

public static partial class Day09 {
    public static void Solve(IEnumerable<string> input) {
        var program = input.Single().ToLongArray();
        new IntCodeComputer(program, [1L]).RunToEnd().GetAwaiter().GetResult().Part1();
        new IntCodeComputer(program, [2L]).RunToEnd().GetAwaiter().GetResult().Part2();
    }
}