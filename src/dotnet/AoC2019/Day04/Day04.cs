using Range = Common.Range;

namespace AoC2019.Day04;

public static partial class Day04 {
    public static void Solve(IEnumerable<string> input) {
        var rangeLimits = input.Single().Split("-").Select(int.Parse).ToArray();
        var range = Range.FromStartAndEndInclusive(rangeLimits[0], rangeLimits[1]);

        range.Count(x => IsValid(x, maxGroup: 6)).Part1();
        range.Count(x => IsValid(x, maxGroup: 2)).Part2();
    }

    private static bool IsValid(int value, int maxGroup) {
        var prev = char.MinValue;
        var count = 1;
        var res = false;
        foreach (var cur in value.ToString()) {
            if (cur < prev) return false;
            if (cur == prev) count++;
            else {
                if (count > 1 && count <= maxGroup) res = true;
                count = 1;
            }

            prev = cur;
        }
        res |= count > 1 && count <= maxGroup;

        return res;
    }
}