namespace AoC2024.Day23;

public static partial class Day23 {

    public static void Solve(IEnumerable<string> input) {
        var adjMap = new Dictionary<string, HashSet<string>>();
        foreach (var edge in input.Select(x => x.Split('-'))) {
            adjMap.GetOrAdd(edge[0], _ => []).Add(edge[1]);
            adjMap.GetOrAdd(edge[1], _ => []).Add(edge[0]);
        }

        FindTrios(adjMap)
            .Count(x => x.Any(t => t.StartsWith('t')))
            .Part1();

        adjMap.FindLargestClique()
            .Order()
            .StringJoin()
            .Part2();
    }

    private static IEnumerable<string[]> FindTrios(Dictionary<string, HashSet<string>> graph) =>
        from a in graph.Keys
        from b in graph[a]
        where string.Compare(a, b, StringComparison.Ordinal) < 0
        from c in graph[b]
        where graph[a].Contains(c) && string.Compare(b, c, StringComparison.Ordinal) < 0
        select new[] { a, b, c };
}
