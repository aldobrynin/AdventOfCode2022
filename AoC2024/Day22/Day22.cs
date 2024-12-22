namespace AoC2024.Day22;

public static partial class Day22 {
    public static void Solve(IEnumerable<string> input) {
        var secrets = input.Select(long.Parse).ToArray();

        secrets
            .Sum(secret => secret.GenerateSequence(Next).ElementAt(2000))
            .Part1();

        FindTotalProfit()
            .Max(v => v.Value)
            .Part2();
        long Next(long current) {
            const int mod = 16777216;
            current = ((current << 6) ^ current) % mod;
            current = ((current >> 5) ^ current) % mod;
            current = ((current << 11) ^ current) % mod;
            return current;
        }

        Dictionary<long, long> FindTotalProfit() {
            var totalProfit = new Dictionary<long, long>();
            foreach (var secret in secrets) {
                var firstPrice = new Dictionary<long, long>();
                var sequences = secret.GenerateSequence(Next)
                    .Take(2001)
                    .Select(x => x % 10)
                    .ZipWithNext((prev, next) => (Price: next, Diff: next - prev))
                    .SlidingWindow(4);

                foreach (var sequence in sequences) {
                    var key = CalcKey(sequence);
                    var price = sequence[^1].Price;
                    if (firstPrice.TryAdd(key, price)) {
                        totalProfit[key] = price + totalProfit.GetValueOrDefault(key);
                    }
                }
            }

            return totalProfit;
        }

        long CalcKey((long Price, long Diff)[] sequence) => sequence.Aggregate(0L, (acc, x) => acc * 100 + x.Diff + 10);
    }
}
