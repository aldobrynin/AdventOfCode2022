namespace AoC2023.Day12;

using CacheItem = (int, int, int);

public partial class Day12 {
    private record ConditionRecord(string Springs, int[] DamagedGroups) {
        public ConditionRecord Unfold(int foldFactor) {
            return new(
                Enumerable.Repeat(Springs, foldFactor).StringJoin("?"),
                DamagedGroups.Repeat(foldFactor).ToArray()
            );
        }
    }

    public static void Solve(IEnumerable<string> input) {
        var maps = input
            .Select(line => line.Split(' '))
            .Select(x => new ConditionRecord(x[0], x[1].ToIntArray()))
            .ToArray();

        maps.AsParallel().Sum(x => CountArrangements(x)).Part1();
        maps.AsParallel().Sum(x => CountArrangements(x.Unfold(5))).Part2();
    }

    private static long CountArrangements(ConditionRecord input, bool useMemo = false) =>
        useMemo ? CountMemoized(input) : CountDp(input);

    private static long CountDp(ConditionRecord input) {
        var (springs, groups) = input;
        var dp = new long[springs.Length + 1];
        var next = new long[springs.Length + 1];
        dp[0] = 1;
        for (var i = 0; i < springs.Length && springs[i] != '#'; i++)
            dp[i + 1] = 1;

        for (var index = 0; index < groups.Length; index++) {
            var groupSize = groups[index];
            next[0] = 0;
            var currentGroup = 0;

            for (var springIndex = 0; springIndex < springs.Length; springIndex++) {
                next[springIndex + 1] = springs[springIndex] is '.' or '?' ? next[springIndex] : 0;
                currentGroup = springs[springIndex] is '#' or '?' ? currentGroup + 1 : 0;

                if (currentGroup < groupSize) continue;
                var groupStartIndex = springIndex - groupSize + 1;
                if (groupStartIndex == 0 || springs[groupStartIndex - 1] != '#')
                    next[springIndex + 1] += groupStartIndex == 0 ? dp[0] : dp[groupStartIndex - 1];
            }

            (next, dp) = (dp, next);
        }

        return dp[^1];
    }

    private static long CountMemoized(ConditionRecord input) {
        var (springs, groups) = input;
        var cache = new Dictionary<CacheItem, long>();
        return Count(springIndex: 0, groupIndex: 0, currentGroup: 0);

        long Count(int springIndex, int groupIndex, int currentGroup) {
            var cacheKey = (springIndex, groupIndex, currentGroup);
            if (cache.TryGetValue(cacheKey, out var cached)) return cached;

            if (springIndex == springs.Length)
                return groups.Length == groupIndex && currentGroup == 0 ||
                       groups.Length - 1 == groupIndex && currentGroup == groups[groupIndex]
                    ? 1
                    : 0;

            var count = 0L;
            if (springs[springIndex] is '#' or '?')
                count += Count(springIndex + 1, groupIndex, currentGroup + 1);

            var isEmptyOrCompleted =
                currentGroup == 0 || groupIndex < groups.Length && currentGroup == groups[groupIndex];
            if (springs[springIndex] is '.' or '?' && isEmptyOrCompleted)
                count += Count(springIndex + 1, groupIndex + Math.Sign(currentGroup), 0);

            return cache[cacheKey] = count;
        }
    }
}