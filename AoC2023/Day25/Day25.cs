namespace AoC2023.Day25;

public static partial class Day25 {
    public record Graph(List<string> Vertices, List<(string From, string To)> Edges) {
        public Dictionary<string, List<string>> ToAdjacencyMap() {
            return Edges.SelectMany(x => new[] { x, (From: x.To, To: x.From) })
                .Where(x => x.From != x.To)
                .GroupBy(x => x.From, x => x.To)
                .ToDictionary(x => x.Key, x => x.ToList());
        }
    }

    public static void Solve(IEnumerable<string> input) {
        var edges = input.Select(line => line.Split(new[] { ':', ' ' }, StringSplitOptions.RemoveEmptyEntries))
            .SelectMany(x => x.Skip(1).Select(f => (From: x[0], To: f)))
            .ToList();

        var vertices = edges.SelectMany(x => new[] { x.From, x.To }).Distinct().ToList();
        var graph = new Graph(vertices, edges);

        foreach (var _ in 1.RangeTo(1000)) {
            var contractedGraph = ContractGraph(graph);
            var removed = graph.Edges.Except(contractedGraph.Edges).ToList();
            if (removed.Count != 3) continue;

            var adj = contractedGraph.ToAdjacencyMap();
            new[] { removed[0].From, removed[0].To }
                .Product(node => SearchHelpers.Bfs(cur => adj[cur], initialStates: [node]).Count())
                .Part1();
            return;
        }

        throw new Exception("No solution found");
    }

    private static Graph ContractGraph(Graph graph) {
        var (vertices, edges) = graph;
        var components = graph.Vertices.ToDictionary(x => x, x => new List<string> { x });
        var vertexToComponent = vertices.ToDictionary(x => x);

        while (components.Count > 2) {
            var (from, to) = edges[Random.Shared.Next(edges.Count)];

            var (fromComponent, toComponent) = (vertexToComponent[from], vertexToComponent[to]);
            if (fromComponent == toComponent) continue;
            if (!components.Remove(toComponent, out var contractedVertices)) continue;
            foreach (var vertex in contractedVertices) {
                vertexToComponent[vertex] = fromComponent;
                components[fromComponent].Add(vertex);
            }
        }

        return graph with { Edges = edges.Where(e => vertexToComponent[e.From] == vertexToComponent[e.To]).ToList() };
    }
}