namespace AoC2019.Day01;

public static partial class Day01 {
    public static void Solve(IEnumerable<string> input) {
        var modules = input.Select(int.Parse).ToArray();
        modules.Sum(GetRequired).Part1();
        modules.Sum(GetRequired2).Part2();

        int GetRequired(int x) => x <= 0 ? 0 : x / 3 - 2;

        int GetRequired2(int x) {
            var total = 0;
            for (var current = GetRequired(x); current > 0; current = GetRequired(current))
                total += current;
            return total;
        }
    }
}