using System.Collections.Frozen;

namespace AoC2023.Day25;

public static partial class Day25 {
    public record Graph(List<string> Vertices, List<(string From, string To)> Edges) {
        public IReadOnlyDictionary<string, int> VertPowers { get; init; } = Vertices.ToDictionary(x => x, _ => 1);
    }

    public static void Solve(IEnumerable<string> input) {
        var edges = input.Select(line => line.Split(new[] { ':', ' ' }, StringSplitOptions.RemoveEmptyEntries))
            .SelectMany(x => x.Skip(1).Select(f => (From: x[0], To: f)))
            .ToList();

        var vertices = edges.SelectMany(x => new[] { x.From, x.To }).Distinct().ToList();
        var graph = new Graph(vertices, edges);

        foreach (var _ in 1.RangeTo(1000)) {
            var contracted = FastMinCut(graph);
            if (contracted.Edges.Count != 3) continue;
            contracted.VertPowers.Product(x => x.Value).Part1();
            return;
        }

        throw new Exception("No solution found");
    }

    private static Graph FastMinCut(Graph graph) {
        if (graph.Vertices.Count <= 6) return MinCut(graph, 2);
        var t = (int)(1 + graph.Vertices.Count / Math.Sqrt(2));
        return Enumerable.Range(0, 2)
            .Select(_ => FastMinCut(MinCut(graph, t)))
            .MinBy(x => x.Edges.Count)!;
    }

    private static Graph MinCut(Graph graph, int t) {
        var (vertices, edges) = graph;
        var components = graph.Vertices.ToDictionary(x => x, x => new List<string> { x });
        var vertexToComponent = vertices.ToDictionary(x => x);

        while (components.Count > t) {
            var (from, to) = edges[Random.Shared.Next(edges.Count)];

            var (fromComponent, toComponent) = (vertexToComponent[from], vertexToComponent[to]);
            if (fromComponent == toComponent) continue;
            if (!components.Remove(toComponent, out var contractedVertices)) continue;
            foreach (var vertex in contractedVertices) {
                vertexToComponent[vertex] = fromComponent;
                components[fromComponent].Add(vertex);
            }
        }

        var newVertices = components.Keys.ToList();
        var newEdges = edges.Select(e => (vertexToComponent[e.From], vertexToComponent[e.To]))
            .Where(x => x.Item1 != x.Item2)
            .ToList();
        var newPowers = components.ToFrozenDictionary(x => x.Key, x => x.Value.Sum(v => graph.VertPowers[v]));
        return new Graph(newVertices, newEdges) { VertPowers = newPowers };
    }
}