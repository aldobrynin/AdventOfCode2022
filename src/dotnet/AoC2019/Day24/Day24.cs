namespace AoC2019.Day24;

public static partial class Day24 {
    public static void Solve(IEnumerable<string> input) {
        var map = Map.From(input);
        var visited = new HashSet<long>();
        Simulate(map).First(rating => !visited.Add(rating)).Part1();

        const int iterations = 200;
        Simulate2(map).ElementAt(iterations).Part2();
    }

    private static IEnumerable<int> Simulate2(Map<char> map) {
        var center = new V(2, 2);
        var state = map.Coordinates().Where(v => map[v] == '#')
            .Select(v => new V3<long>(v.X, v.Y, 0))
            .ToHashSet();

        while (true) {
            yield return state.Count;
            var adjCount = state.SelectMany(Neighbors).CountFrequency();

            state = adjCount
                .Where(kv => kv.Value == 1 || kv.Value == 2 && !state.Contains(kv.Key))
                .Select(kv => kv.Key)
                .ToHashSet();
        }

        IEnumerable<V3<long>> Neighbors(V3<long> position) {
            var (x, y, z) = position;
            foreach (var dir in V.Directions4) {
                var next = new V(x, y) + dir;
                if (!map.Contains(next)) yield return new V3<long>(center.X + dir.X, center.Y + dir.Y, z - 1);
                else if (next == center) {
                    foreach (var i in map.RowIndices) {
                        if (dir == V.Right) yield return new V3<long>(0, i, z + 1);
                        else if (dir == V.Left) yield return new V3<long>(map.LastColumnIndex, i, z + 1);
                        else if (dir == V.Up) yield return new V3<long>(i, map.LastRowIndex, z + 1);
                        else if (dir == V.Down) yield return new V3<long>(i, 0, z + 1);
                    }
                }
                else yield return new V3<long>(next.X, next.Y, z);
            }
        }
    }

    private static IEnumerable<long> Simulate(Map<char> map) {
        while (true) {
            yield return GetBiodiversityRating(map);
            map = Next(map);
        }
    }

    private static Map<char> Next(Map<char> map) {
        var newMap = map.Clone();
        var adjCount = map.FindAll('#').SelectMany(map.Area4).CountFrequency();
        foreach (var coordinate in map.Coordinates()) {
            newMap[coordinate] = (map[coordinate], adjCount.GetValueOrDefault(coordinate)) switch {
                ('#', not 1) => '.',
                ('.', 1 or 2) => '#',
                _ => newMap[coordinate],
            };
        }

        return newMap;
    }

    private static long GetBiodiversityRating(Map<char> map) {
        return map.FindAll('#')
            .Select(c => c.Y * map.SizeX + c.X)
            .Aggregate(0L, (acc, i) => acc.SetBit((int)i));
    }
}