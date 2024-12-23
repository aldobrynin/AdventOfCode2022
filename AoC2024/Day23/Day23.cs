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

        FindLargestClique(adjMap)
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

    private static HashSet<string> FindLargestClique(Dictionary<string, HashSet<string>> graph) {
        var cliques = BronKerbosch([], graph.Keys.ToHashSet(), [], graph);

        return cliques.MaxBy(clique => clique.Count)!;
    }

    private static List<HashSet<string>> BronKerbosch(
        HashSet<string> current,
        HashSet<string> candidates,
        HashSet<string> visited,
        Dictionary<string, HashSet<string>> graph) {

        if (candidates.Count == 0 && visited.Count == 0) {
            return [current];
        }

        var cliques = new List<HashSet<string>>();
        foreach (var candidate in candidates) {
            var next = current.Append(candidate).ToHashSet();
            var nextCandidates = candidates.Intersect(graph[candidate]).ToHashSet();
            var nextVisited = visited.Intersect(graph[candidate]).ToHashSet();

            cliques.AddRange(BronKerbosch(next, nextCandidates, nextVisited, graph));

            candidates.Remove(candidate);
            visited.Add(candidate);
        }

        return cliques;
    }
}
