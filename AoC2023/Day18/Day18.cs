namespace AoC2023.Day18;

public static partial class Day18 {
    public static void Solve(IEnumerable<string> input) {
        var lines = input.ToArray();
        Solve(lines.Select(Parse1)).Part1();
        Solve(lines.Select(Parse2)).Part2();
    }

    private static long Solve(IEnumerable<V> moves) {
        var polygon = moves.RunningFold(V.Zero, (prev, move) => prev + move).ToArray();
        var boundary = polygon.ZipWithNext((a, b) => 1L * a.DistTo(b)).Sum();
        // A = i + b/2 - 1 => i = A - b/2 + 1
        var interiorPointsCount = ShoelaceArea(polygon) - boundary / 2 + 1;
        return boundary + interiorPointsCount;
    }

    private static long ShoelaceArea(IEnumerable<V> polygon) {
        var area = polygon.ZipWithNext((a, b) => 1L * a.X * b.Y - 1L * a.Y * b.X).Sum();
        return Math.Abs(area) / 2;
    }

    private static V Parse1(string line) {
        var segments = line.Split(' ');
        var dir = segments[0] switch {
            "R" => V.Right,
            "L" => V.Left,
            "U" => V.Up,
            "D" => V.Down,
            _ => throw new ArgumentException($"Invalid direction: {segments[0]}")
        };
        return (dir * segments[1].ToInt());
    }

    private static V Parse2(string line) {
        var segments = line.Split(' ').Last().Trim('(', ')', '#');
        List<V> dirs = [V.Right, V.Down, V.Left, V.Up];
        return (dirs[segments[^1] - '0'] * Convert.ToInt32(segments[..^1], fromBase: 16));
    }
}