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
        if (useConsoleInput) PrintMap(map);

        foreach (var move in moveSequence.Select(V.FromArrow)) {
            var piecesToMove = FindPiecesToMove(move, map, robot);

            var updates = piecesToMove
                .Select(v => (NewPosition: v + move, Value: map[v]))
                .ToArray();

            foreach (var oldPosition in piecesToMove) map[oldPosition] = '.';
            foreach (var (newPosition, value) in updates) map[newPosition] = value;

            if (piecesToMove.Length != 0) robot += move;
            if (useConsoleInput) PrintMap(map);
        }

        return map;
    }

    private static V[] FindPiecesToMove(V move, Map<char> map, V robot) {
        var result = new List<V[]>();
        var piecesToMove = new[] { robot };
        while (piecesToMove.Length != 0 && piecesToMove.All(IsNotWall)) {
            result.Add(piecesToMove);
            piecesToMove = piecesToMove
                .SelectMany<V, V>(v =>
                    (map[v + move], move) switch {
                        ('[', { X: 0 }) => [v + move, v + move + V.Right],
                        (']', { X: 0 }) => [v + move + V.Left, v + move],
                        ('.', _) => [],
                        _ => [v + move],
                    })
                .ToArray();
        }
        return piecesToMove.All(IsNotWall) ? result.Flatten().ToArray() : [];

        bool IsNotWall(V v) => map[v] is not '#';
    }

    private static Map<char> Scale(Map<char> map) {
        var newMap = new Map<char>(map.SizeX * 2, map.SizeY);
        foreach (var coordinate in map.Coordinates()) {
            newMap[coordinate with { X = coordinate.X * 2 }] = map[coordinate] switch {
                'O' => '[',
                _ => map[coordinate],
            };
            newMap[coordinate with { X = coordinate.X * 2 + 1 }] = map[coordinate] switch {
                'O' => ']',
                '#' => '#',
                _ => '.',
            };
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

    private static void PrintMap(Map<char> map) =>
        map.PrintColored(v => (map[v].ToString(), map[v] switch {
            '[' or ']' => ConsoleColor.Green,
            'O' => ConsoleColor.Green,
            '@' => ConsoleColor.Yellow,
            _ => ConsoleColor.White,
        }));
}
