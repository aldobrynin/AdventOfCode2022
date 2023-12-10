using Range = Common.Range;

namespace AoC2023.Day10;

public class Day10 {
    private static readonly Dictionary<char, V[]> Tiles =
        new() {
            ['|'] = new[] { V.Up, V.Down, },
            ['-'] = new[] { V.Left, V.Right, },
            ['L'] = new[] { V.Right, V.Down, },
            ['J'] = new[] { V.Left, V.Down, },
            ['F'] = new[] { V.Up, V.Right, },
            ['7'] = new[] { V.Left, V.Up, },
            ['.'] = Array.Empty<V>(),
        };

    public static void Solve(IEnumerable<string> input) {
        var map = Map.From(input);
        var start = map.FindAll('S').Single();
        var (startTile, _) = Tiles
            .Single(kv => kv.Value.Count(dir => AreTilesConnected(kv.Key, map[start + dir], dir)) == 2)
            .Dump("Possible start tiles: ");
        map[start] = startTile;

        var pipe = map
            .Bfs((from, to) => AreTilesConnected(map[from], map[to], to - from), start)
            .Select(x => x.State)
            .ToHashSet();
        (pipe.Count / 2).Dump("Part1: ");
        FindIsolatedCells(map, pipe).Dump("Part2: ");
    }

    private static int FindIsolatedCells(Map<char> map, HashSet<V> pipe) {
        const int factor = 2;
        var (enlargedMap, enlargedPipe) = EnlargeMapAndPipe(map, pipe, factor);

        var outer = enlargedMap.BorderCoordinates().Where(v => !enlargedPipe.Contains(v)).ToArray();
        var enlargedOuterCells = enlargedMap.Bfs((_, to) => !enlargedPipe.Contains(to), outer)
            .Select(x => x.State)
            .ToHashSet();
        if (AoCContext.IsSample) PrintColored(enlargedMap, enlargedPipe, enlargedOuterCells);

        var outerCells = enlargedOuterCells
            .Where(v => v % factor == V.Zero)
            .Select(v => v / factor)
            .ToHashSet();
        if (AoCContext.IsSample) PrintColored(map, pipe, outerCells);

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

    private static void PrintColored(Map<char> map, IReadOnlySet<V> pipe, IReadOnlySet<V>? outerCells = null) {
        var isolatedCells = map.Coordinates().Except(pipe).Except(outerCells ?? new HashSet<V>()).ToHashSet();
        var printMap = new Dictionary<char, string> {
            ['|'] = "│",
            ['-'] = "─",
            ['F'] = "┌",
            ['7'] = "┐",
            ['L'] = "└",
            ['J'] = "┘",
        };
        map.PrintColored(Printer);

        return;

        (string, ConsoleColor?) Printer(V v) =>
            (pipe.Contains(v), outerCells?.Contains(v), isolatedCells.Contains(v)) switch {
                (true, _, _) => (printMap[map[v]], ConsoleColor.Green),
                (_, true, _) => ("O", ConsoleColor.Yellow),
                (_, _, true) => ("I", ConsoleColor.Red),
                _ => (map[v].ToString(), null)
            };
    }
}