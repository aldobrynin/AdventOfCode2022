namespace AoC2024.Day19;

public static partial class Day19 {
    public static void Solve(IEnumerable<string> input) {
        var sections = input.SplitBy(string.IsNullOrWhiteSpace).ToArray();
        var towels = sections[0].Single().Split([' ', ','], StringSplitOptions.RemoveEmptyEntries).ToArray();

        var designs = sections[1];

        designs.Count(x => CountWays(x) > 0).Part1();
        designs.Sum(CountWays).Part2();

        long CountWays(string design) {
            var dp = new long[design.Length + 1];
            dp[0] = 1;
            for (var i = 1; i <= design.Length; i++) {
                foreach (var towel in towels) {
                    if (ContainsPattern(design, towel, i - towel.Length)) {
                        dp[i] += dp[i - towel.Length];
                    }
                }
            }

            return dp[^1];
        }

        bool ContainsPattern(string design, string pattern, int startIndex) {
            if (startIndex < 0) return false;
            for (var i = 0; i < pattern.Length; i++) {
                if (design[startIndex + i] != pattern[i])
                    return false;
            }

            return true;
        }
    }
}
