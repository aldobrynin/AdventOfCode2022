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

        maps.Sum(Count).Part1();
        maps.Sum(x => Count(x.Unfold(5))).Part2();
    }

    private static long Count(ConditionRecord input) => CountMemoized(input, cache: new());

    private static long CountMemoized(ConditionRecord input,
        int springIndex = 0, int groupIndex = 0, int currentGroup = 0, Dictionary<CacheItem, long> cache = null!) {

        var cacheKey = (springIndex, groupIndex, currentGroup);
        if (cache.TryGetValue(cacheKey, out var cached)) return cached;
        var (springs, groups) = input;

        if (springIndex == springs.Length)
            return groups.Length == groupIndex && currentGroup == 0 ||
                   groups.Length - 1 == groupIndex && currentGroup == groups[groupIndex]
                ? 1
                : 0;

        var count = 0L;
        if (springs[springIndex] is '#' or '?')
            count += CountMemoized(input, springIndex + 1, groupIndex, currentGroup + 1, cache);

        var isEmptyOrCompleted = currentGroup == 0 || groupIndex < groups.Length && currentGroup == groups[groupIndex];
        if (springs[springIndex] is '.' or '?' && isEmptyOrCompleted)
            count += CountMemoized(input, springIndex + 1, groupIndex + Math.Sign(currentGroup), 0, cache);

        return cache[cacheKey] = count;
    }
}