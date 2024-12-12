namespace AoC2024.Day12;

public static partial class Day12 {
    public static void Solve(IEnumerable<string> input) {
        var map = Map.From(input);
        var visited = new HashSet<V>();
        var regions = new List<IReadOnlySet<V>>();

        foreach (var coordinate in map.Coordinates()) {
            if (!visited.Add(coordinate)) continue;

            var region = VisitRegion(map, coordinate);
            regions.Add(region);
            visited.UnionWith(region);
        }

        regions.Sum(CalculatePrice).Part1();
        regions.Sum(CalculatePrice2).Part2();

        int CalculatePrice(IReadOnlyCollection<V> region) {
            var area = region.Count;
            var perimeter = region.SelectMany(x => x.Area4())
                .Count(x => map.GetValueOrDefault(x) != map[region.First()]);
            return area * perimeter;
        }

        int CalculatePrice2(IReadOnlySet<V> region) {
            var area = region.Count;
            var sides = region.Sum(v => CountAngles(v, region));
            return area * sides;
        }
    }

    private static HashSet<V> VisitRegion(Map<char> map, V start) {
        return map.Bfs((_, to) => map[to] == map[start], start)
            .Select(x => x.State)
            .ToHashSet();
    }

    private static int CountAngles(V v, IReadOnlySet<V> region) {
        var directions = new[] {
            V.Up + V.Right,
            V.Up + V.Left,
            V.Down + V.Right,
            V.Down + V.Left,
        };

        return directions.Count(dir => IsAngle(region, v, dir));
    }

    private static bool IsAngle(IReadOnlySet<V> region, V point, V direction) {
        var xProjection = point + direction with { Y = 0 };
        var yProjection = point + direction with { X = 0 };
        return (!region.Contains(yProjection) && !region.Contains(xProjection))
               ||
               (region.Contains(yProjection) && region.Contains(xProjection) && !region.Contains(point + direction));
    }
}
