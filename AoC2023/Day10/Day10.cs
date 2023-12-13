using Range = Common.Range;

namespace AoC2023.Day10;

public partial class Day10 {
    public static void Solve(IEnumerable<string> input) {
        var map = Map.From(input);
        var start = map.FindAll('S').Single();
        var longestLoop = map.Dfs((from, to) => AreTilesConnected(map[from], map[to], to - from), start)
            .Where(x => x.Distance > 1 && start.Area4().Contains(x.State))
            .MaxBy(x => x.Distance) ?? throw new Exception("No loop found");
        ((longestLoop.Distance + 1) / 2).Part1();
        var pipe = longestLoop.FromStart().ToArray();

        var adjacentToStart = new[] { pipe[1], longestLoop.State };
        var startTile = Tiles.Keys
            .Where(x => x != 'S')
            .Single(tile => adjacentToStart.All(n => AreTilesConnected(map[n], tile, start - n)));
        map[start] = startTile;
        CountPolygonInteriorPoints(pipe).Part2();
        FindIsolatedCells(map, pipe.ToHashSet()).Part2();
    }

    // Shoelace formula: https://en.wikipedia.org/wiki/Pick%27s_theorem
    // + Pick's theorem: https://en.wikipedia.org/wiki/Pick%27s_theorem
    private static long CountPolygonInteriorPoints(IReadOnlyList<V> polygon) {
        var polygonArea = polygon
            .Zip(polygon.Skip(1).Append(polygon[0]), (a, b) => a.X * b.Y - a.Y * b.X)
            .Sum();
        polygonArea = Math.Abs(polygonArea / 2);

        return polygonArea - polygon.Count / 2 + 1;
    }

    private static int FindIsolatedCells(Map<char> map, HashSet<V> pipe) {
        const int factor = 2;
        var (enlargedMap, enlargedPipe) = EnlargeMapAndPipe(map, pipe, factor);

        var outer = enlargedMap.BorderCoordinates().Where(v => !enlargedPipe.Contains(v)).ToArray();
        var enlargedOuterCells = enlargedMap.Bfs((_, to) => !enlargedPipe.Contains(to), outer)
            .Select(x => x.State)
            .ToHashSet();

        var outerCells = enlargedOuterCells
            .Where(v => v % factor == V.Zero)
            .Select(v => v / factor)
            .ToHashSet();
        // if (AoCContext.IsSample) PrintColored(map, pipe, outerCells);

        return map.SizeY * map.SizeX - pipe.Count - outerCells.Count;
    }

    private static (Map<char>, HashSet<V>) EnlargeMapAndPipe(Map<char> map, IEnumerable<V> pipe, int factor) {
        var enlargedMap = new Map<char>(map.SizeX * factor, map.SizeY * factor);
        foreach (var newCoordinate in enlargedMap.Coordinates()) {
            enlargedMap[newCoordinate] = newCoordinate % factor == V.Zero ? map[newCoordinate / factor] : '.';
        }

        var enlargedPipe = pipe.Select(x => x * factor).ToHashSet();
        var pipeExtension = enlargedPipe.SelectMany(p => Tiles[enlargedMap[p]]
                .SelectMany(dir =>
                    Range.FromStartAndEnd(1, factor).Select(x => (Pos: p + dir * x, Tile: dir.X == 0 ? '|' : '-'))))
            .DistinctBy(x => x.Pos)
            .ToArray();
        foreach (var (pos, tile) in pipeExtension) {
            enlargedPipe.Add(pos);
            enlargedMap[pos] = tile;
        }

        return (enlargedMap, enlargedPipe);
    }

    private static bool AreTilesConnected(char from, char to, V dir) =>
        Tiles[from].Contains(dir) && Tiles[to].Contains(-dir);

    private static readonly Dictionary<char, V[]> Tiles =
        new() {
            ['|'] = new[] { V.Up, V.Down, },
            ['-'] = new[] { V.Left, V.Right, },
            ['L'] = new[] { V.Right, V.Up, },
            ['J'] = new[] { V.Left, V.Up, },
            ['F'] = new[] { V.Down, V.Right, },
            ['7'] = new[] { V.Left, V.Down, },
            ['S'] = V.Directions4,
            ['.'] = Array.Empty<V>(),
        };

    private static readonly Dictionary<char, string> PrintMap = new() {
        ['|'] = "│",
        ['-'] = "─",
        ['F'] = "┌",
        ['7'] = "┐",
        ['L'] = "└",
        ['J'] = "┘",
        ['S'] = "S",
        ['.'] = ".",
    };

    private static void PrintColored(Map<char> map, IReadOnlySet<V> pipe, IReadOnlySet<V>? outerCells = null) {
        var isolatedCells = map.Coordinates().Except(pipe).Except(outerCells ?? new HashSet<V>()).ToHashSet();
        map.PrintColored(FormatCell);

        (string, ConsoleColor?) FormatCell(V v) =>
            (pipe.Contains(v), outerCells?.Contains(v), isolatedCells.Contains(v)) switch {
                (true, _, _) => (PrintMap[map[v]], ConsoleColor.Green),
                (_, true, _) => ("O", ConsoleColor.Yellow),
                (_, _, true) => ("I", ConsoleColor.Red),
                _ => (PrintMap[map[v]], null)
            };
    }
}