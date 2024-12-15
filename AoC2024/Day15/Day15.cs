namespace AoC2024.Day15;

public static partial class Day15 {
    public static void Solve(IEnumerable<string> input) {
        var sections = input.SplitBy(string.IsNullOrWhiteSpace).ToArray();
        var moves = sections[1].Flatten().ToArray();

        var map = Map.From(sections[0]);
        MoveBoxes(map, moves)
            .FindAll('O')
            .Sum(v => v.X + 100 * v.Y)
            .Part1();

        MoveBoxes(Scale(map), moves)
            .FindAll('[')
            .Sum(v => v.X + 100 * v.Y)
            .Part2();
    }

    private static Map<char> MoveBoxes(Map<char> map, char[] moves) {
        map = map.Clone();
        var robot = map.FindFirst('@');
        var useConsoleInput = false;
        var moveSequence = useConsoleInput ? ReadMovesFromConsole() : moves;

        if (useConsoleInput) PrintMap();
        foreach (var move in moveSequence.Select(V.FromArrow)) {
            var updates = FindPiecesToMove(move, map, robot)
                .Select(v => (OldPosition: v, NewPosition: v + move, Value: map[v]))
                .ToArray();
            if (updates.Any(u => map[u.NewPosition] == '#')) continue;

            foreach (var (oldPosition, _, _) in updates) map[oldPosition] = '.';
            foreach (var (_, newPosition, value) in updates) map[newPosition] = value;

            robot += move;
            if (useConsoleInput) PrintMap();
        }

        if (useConsoleInput) PrintMap();
        return map;

        void PrintMap() => map.PrintColored(v => (map[v].ToString(), map[v] switch {
            '[' or ']' => ConsoleColor.Green,
            'O' => ConsoleColor.Green,
            '@' => ConsoleColor.Yellow,
            _ => ConsoleColor.White,
        }));
    }

    private static V[] FindPiecesToMove(V move, Map<char> map, V robot) {
        if (move is { Y: 0 }) {
            return EnumeratePoints(map, robot, move)
                .TakeWhile(IsNotEmptyOrWall)
                .ToArray();
        }

        var result = new List<V>();
        var currentLayer = new[] { robot };
        while (currentLayer.Any(IsNotEmptyOrWall)) {
            result.AddRange(currentLayer);
            currentLayer = currentLayer
                .Select(x => x + move)
                .SelectMany<V, V>(v => map[v] switch {
                    '[' => [v, v + V.Right],
                    ']' => [v + V.Left, v],
                    '.' => [],
                    _ => [v],
                })
                .ToArray();
        }

        return result.ToArray();

        bool IsNotEmptyOrWall(V v) => map[v] is not '.' and not '#';
    }

    private static Map<char> Scale(Map<char> map) {
        var newMap = new Map<char>(map.SizeX * 2, map.SizeY);
        foreach (var coordinate in map.Coordinates()) {
            switch (map[coordinate]) {
                case 'O':
                    newMap[coordinate with { X = coordinate.X * 2 }] = '[';
                    newMap[coordinate with { X = coordinate.X * 2 + 1 }] = ']';
                    break;
                case '#':
                    newMap[coordinate with { X = coordinate.X * 2 }] = '#';
                    newMap[coordinate with { X = coordinate.X * 2 + 1 }] = '#';
                    break;
                default:
                    newMap[coordinate with { X = coordinate.X * 2 }] = map[coordinate];
                    newMap[coordinate with { X = coordinate.X * 2 + 1 }] = '.';
                    break;
            }
        }

        return newMap;
    }

    private static IEnumerable<char> ReadMovesFromConsole() {
        while (true) {
            var key = Console.ReadKey();
            if (key.Key == ConsoleKey.Escape) yield break;

            var arrow = key switch {
                { Key: ConsoleKey.UpArrow } => '^',
                { Key: ConsoleKey.DownArrow } => 'v',
                { Key: ConsoleKey.LeftArrow } => '<',
                { Key: ConsoleKey.RightArrow } => '>',
                _ => char.MinValue,
            };
            if (arrow != char.MinValue) yield return arrow;
        }
    }

    private static IEnumerable<V> EnumeratePoints(Map<char> map, V start, V direction) {
        for (var pos = start; map.Contains(pos); pos += direction) {
            yield return pos;
        }
    }
}
