namespace AoC2018.Day25;

public static partial class Day25 {
    public record V4(int X, int Y, int Z, int W) {
        public V4(int[] coords) : this(coords[0], coords[1], coords[2], coords[3]) {
        }

        public int DistTo(V4 other) => Math.Abs(X - other.X) +
                                       Math.Abs(Y - other.Y) +
                                       Math.Abs(Z - other.Z) +
                                       Math.Abs(W - other.W);
    }

    public static void Solve(IEnumerable<string> input) {
        var points = input.Select(line => line.ToIntArray())
            .Select(coords => new V4(coords))
            .ToArray();

        var adjMap = new Dictionary<V4, HashSet<V4>>();
        foreach (var pairs in points.Combinations(2).Where(x => x[0].DistTo(x[1]) <= 3)) {
            adjMap.GetOrAdd(pairs[0], _ => []).Add(pairs[1]);
            adjMap.GetOrAdd(pairs[1], _ => []).Add(pairs[0]);
        }

        var constellations = new List<HashSet<V4>>();
        var visited = new HashSet<V4>();
        foreach (var point in points) {
            if (visited.Contains(point)) continue;
            var connected = SearchHelpers
                .Bfs(x => adjMap.GetValueOrDefault(x, []), initialStates: point)
                .Select(x => x.State)
                .ToHashSet();
            visited.UnionWith(connected);
            constellations.Add(connected);
        }

        constellations.Count.Part1();
    }
}
