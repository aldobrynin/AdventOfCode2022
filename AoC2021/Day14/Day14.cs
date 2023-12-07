namespace AoC2021.Day14;

public class Day14
{
    public static void Solve(IEnumerable<string> input)
    {
        var array = input.ToArray();
        var template = array.First();

        var insertions = array.Skip(2).ToArray();
        Apply(template, insertions, 10).Dump("Part1: ");
        Apply(template, insertions, 40).Dump("Part2: ");
    }

    private static long Apply(string template, string[] insertions, int transformsCount)
    {
        var transformRules = insertions
            .Select(x => x.Split(new[] { ' ', '-', '>' }, StringSplitOptions.RemoveEmptyEntries))
            .ToDictionary(x => x[0], x => (IEnumerable<string>)new[] { $"{x[0][0]}{x[1]}", $"{x[1]}{x[0][1]}" });

        var pairsCount = template.Indices()
            .SkipLast(1)
            .Select(ind => template.Substring(ind, 2))
            .GroupBy(x => x)
            .ToDictionary(x => x.Key, x => x.LongCount());

        for (var i = 0; i < transformsCount; i++)
            pairsCount = TransformCounts(pairsCount, transformRules);

        var result = pairsCount
            .SelectMany(x => x.Key.Select(c => (Char: c, Count: x.Value)))
            .GroupBy(x => x.Char)
            .Select(x =>
            {
                var totalCount = x.Sum(s => s.Count);
                if (x.Key == template[0]) totalCount++;
                if (x.Key == template[^1]) totalCount++;
                return totalCount / 2;
            }).ToArray();

        return result.Max() - result.Min();
    }

    private static Dictionary<string, long> TransformCounts(
        IReadOnlyDictionary<string, long> prevCounts,
        IReadOnlyDictionary<string, IEnumerable<string>> transformRules)
    {
        var result = new Dictionary<string, long>(prevCounts.Count);
        foreach (var (pair, value) in prevCounts)
        foreach (var transformedPair in transformRules.GetValueOrDefault(pair, Enumerable.Repeat(pair, 1)))
            result[transformedPair] = result.GetValueOrDefault(transformedPair) + value;

        return result;
    }
}