namespace AoC2024.Day22;

public static partial class Day22 {
    public static void Solve(IEnumerable<string> input) {
        var secrets = input.Select(long.Parse).ToArray();

        secrets
            .Sum(secret => secret.GenerateSequence(Next).ElementAt(2000))
            .Part1();

        FindTotalProfit()
            .Part2();

        long Next(long current) {
            const int mod = 16777216;
            current = ((current << 6) ^ current) % mod;
            current = ((current >> 5) ^ current) % mod;
            current = ((current << 11) ^ current) % mod;
            return current;
        }

        long FindTotalProfit() {
            var visited = new int[20 * 20 * 20 * 20];
            var totalProfit = new int[20 * 20 * 20 * 20];
            var max = 0L;
            foreach (var secret in secrets) {
                var sequences = secret
                    .GenerateSequence(Next)
                    .Take(2001)
                    .Select(x => (int)(x % 10))
                    .ZipWithNext((prev, next) => (Price: next, Diff: next - prev))
                    .SlidingWindow(4)
                    .Select(x => (Key: CalcKey(x), x[^1].Price));

                foreach (var (key, price) in sequences) {
                    if (visited[key] == secret) continue;

                    totalProfit[key] += price;
                    visited[key] = (int)secret;
                    max = Math.Max(max, totalProfit[key]);
                }
            }

            return max;
        }

        long CalcKey((int Price, int Diff)[] sequence) =>
            sequence.Aggregate(0L, (acc, x) => acc * 19 + x.Diff + 9);
    }
}
