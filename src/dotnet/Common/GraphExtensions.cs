using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;

namespace Common;

public static class GraphExtensions {
    public static IEnumerable<IReadOnlySet<TNode>> FindCliques<TNode>(
        this IReadOnlyDictionary<TNode, HashSet<TNode>> graph) where TNode : notnull {
        return BronKerbosch([], graph.Keys.ToImmutableHashSet(), [], graph);
    }

    public static IReadOnlySet<TNode> FindLargestClique<TNode>(this IReadOnlyDictionary<TNode, HashSet<TNode>> graph)
        where TNode : notnull {
        return graph.FindCliques().MaxBy(clique => clique.Count)!;
    }

    // ReSharper disable once IdentifierTypo
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    private static IEnumerable<IReadOnlySet<TNode>> BronKerbosch<TNode>(
        ImmutableHashSet<TNode> R,
        ImmutableHashSet<TNode> P,
        ImmutableHashSet<TNode> X,
        IReadOnlyDictionary<TNode, HashSet<TNode>> graph) where TNode : notnull {

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
                    ) {
                yield return next;
            }

            P = P.Remove(candidate);
            X = X.Add(candidate);
        }
    }
}
