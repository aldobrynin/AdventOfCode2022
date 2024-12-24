using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;

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

    private static IReadOnlySet<string> FindLargestClique(Dictionary<string, HashSet<string>> graph) {
        return BronKerbosch([], graph.Keys.ToImmutableHashSet(), [], graph)
            .MaxBy(clique => clique.Count)!;
    }

    [SuppressMessage("ReSharper", "InconsistentNaming")]
    private static IEnumerable<IReadOnlySet<string>> BronKerbosch(
        ImmutableHashSet<string> R,
        ImmutableHashSet<string> P,
        ImmutableHashSet<string> X,
        Dictionary<string, HashSet<string>> graph) {

        if (P.Count == 0 && X.Count == 0) {
            yield return R;
            yield break;
        }

        var pivot = P.Union(X).MaxBy(v => graph[v].Count)!;
        var candidates = P.Except(graph[pivot]);

        foreach (var candidate in candidates) {
            foreach (var next in BronKerbosch(
                         R: R.Add(candidate),
                         P: P.Intersect(graph[candidate]),
                         X: X.Intersect(graph[candidate]),
                         graph
                     )
                    )
                yield return next;

            P = P.Remove(candidate);
            X = X.Add(candidate);
        }
    }
}
