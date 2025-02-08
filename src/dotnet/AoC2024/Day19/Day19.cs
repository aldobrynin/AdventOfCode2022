namespace AoC2024.Day19;

public static partial class Day19 {
    public static void Solve(IEnumerable<string> input) {
        var sections = input.SplitBy(string.IsNullOrWhiteSpace).ToArray();
        var towels = sections[0].Single().Split([' ', ','], StringSplitOptions.RemoveEmptyEntries).ToArray();

        var designs = sections[1];

        var trie = Trie.FromWords(towels);
        var counts = designs.Select(CountWays).ToArray();
        counts.Count(x => x > 0).Part1();
        counts.Sum().Part2();

        long CountWays(string design) {
            var dp = new long[design.Length + 1];
            dp[0] = 1;
            for (var i = 0; i < design.Length; i++) {
                if (dp[i] == 0) continue;
                foreach (var match in trie.FindPrefixes(design[i..])) {
                    dp[i + match.Length] += dp[i];
                }
            }

            return dp[^1];
        }
    }
}
