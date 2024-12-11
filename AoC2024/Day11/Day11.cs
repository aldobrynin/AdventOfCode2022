namespace AoC2024.Day11;

public static partial class Day11 {
    public static void Solve(IEnumerable<string> input) {
        var stones = input.Single().ToLongArray();

        Simulate(stones)
            .ElementAt(25)
            .Part1();

        Simulate(stones)
            .ElementAt(75)
            .Part2();

        IEnumerable<long> Simulate(long[] current) {
            var cache = current.CountFrequency();

            while (true) {
                yield return cache.Sum(x => x.Value);

                cache = cache
                    .SelectMany(x => Split(x.Key).Select(stone => (stone, x.Value)))
                    .GroupBy(x => x.stone)
                    .ToDictionary(x => x.Key, x => x.Sum(y => y.Value));
            }
        }

        long[] Split(long stone) {
            if (stone == 0) return [1];
            var digits = DigitsCount(stone);
            if (digits % 2 == 0) {
                var p = (int)Math.Pow(10, digits / 2);
                return [stone / p, stone % p];
            }

            return [stone * 2024];
        }

        int DigitsCount(long stone) {
            return stone == 0 ? 1 : (int)Math.Log10(stone) + 1;
        }
    }
}
