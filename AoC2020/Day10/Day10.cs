namespace AoC2020.Day10;

public class Day10
{
    public static void Solve(IEnumerable<string> input) {
        var jolts = input.Select(int.Parse).Order().Prepend(0).ToList();
        jolts.Add(jolts.Last() + 3);
        Part1(jolts);
        Part2(jolts);
    }

    private static void Part1(IReadOnlyCollection<int> jolts) {
        var lookup = jolts.Zip(jolts.Skip(1), (a, b) => b - a).ToLookup(x => x);
        (lookup[1].Count() * lookup[3].Count()).Dump("Part1: ");
    }

    private static void Part2(IReadOnlyList<int> jolts) {
        var hashSet = jolts.ToHashSet();
        var maxValue = jolts[^1];
        var dp = new long[maxValue + 1];
        dp[0] = 1;
        for (var i = 1; i <= maxValue; i++) {
            for (var diff = 1; diff <= 3 && diff <= i; diff++) {
                if (hashSet.Contains(i - diff))
                    dp[i] += dp[i - diff];
            }
        }

        var res = dp.Last();
        res.Dump("Part2: ");
    }
}