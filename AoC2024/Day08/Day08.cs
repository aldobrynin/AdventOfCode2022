namespace AoC2024.Day08;

public static partial class Day08 {
    public static void Solve(IEnumerable<string> input) {
        var map = Map.From(input);
        var pairs = map.FindAll(x => x is not '.').ToLookup(x => map[x])
            .SelectMany(x => x.Combinations(k: 2))
            .Select(x => (A: x[0], B: x[1]))
            .ToArray();

        pairs
            .SelectMany(x => new[] { Line(x.B, x.B - x.A), Line(x.A, x.A - x.B) })
            .SelectMany(x => x.Skip(1).TakeWhile(map.Contains).Take(1))
            .Distinct()
            .Count()
            .Part1();

        pairs
            .SelectMany(x => new[] { Line(x.B, x.B - x.A), Line(x.A, x.A - x.B) })
            .SelectMany(x => x.TakeWhile(map.Contains))
            .Distinct()
            .Count()
            .Part2();
    }

    private static IEnumerable<V> Line(V from, V dir) {
        while (true) {
            yield return from;
            from += dir;
        }
    }
}
