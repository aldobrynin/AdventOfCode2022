using System.Text.RegularExpressions;
using Range = Common.Range;

namespace AoC2018.Day10;

public static partial class Day10 {
    public record Point(V Position, V Velocity) {
        private static readonly Regex Regex = new(@"<(?<position>[\d\s,\-]+)>");

        public Point Move() => this with { Position = Position + Velocity };

        public static Point Parse(string input) {
            var match = Regex.Matches(input);
            return new Point(V.Parse(match[0].Groups[1].Value), V.Parse(match[1].Groups[1].Value));
        }
    }

    public static void Solve(IEnumerable<string> input) {
        var initial = input.Select(Point.Parse).ToArray();

        var (points, second, _) = initial
            .GenerateSequence(prev => prev.Select(p => p.Move()).ToArray())
            .Select((x, second) => (Points: x, Square: GetSquare(x), Second: second))
            .ZipWithNext((a, b) => (b.Points, b.Second, SquareDiff: b.Square - a.Square))
            .TakeWhile(x => x.SquareDiff < 0)
            .Last();

        Print(points);
        second.Part2();
    }

    private static void Print(Point[] points) {
        var (minX, maxX, minY, maxY) = GetBounds(points.Select(p => p.Position));
        var rangeX = Range.FromStartAndEndInclusive(minX, maxX);

        var set = points.Select(x => x.Position).ToHashSet();
        minY.RangeTo(maxY)
            .Select(y => rangeX.Select(x => set.Contains(new V(x, y)) ? '#' : '.').StringJoin(string.Empty))
            .StringJoin(Environment.NewLine)
            .Dump();
    }

    private static long GetSquare(Point[] points) {
        var (minX, maxX, minY, maxY) = GetBounds(points.Select(p => p.Position));
        return (maxX - minX) * (maxY - minY);
    }

    private static (long MinX, long MaxX, long MinY, long MaxY) GetBounds(IEnumerable<V> values) {
        long minX = long.MaxValue, maxX = long.MinValue, minY = long.MaxValue, maxY = long.MinValue;
        foreach (var value in values) {
            minX = Math.Min(minX, value.X);
            maxX = Math.Max(maxX, value.X);
            minY = Math.Min(minY, value.Y);
            maxY = Math.Max(maxY, value.Y);
        }

        return (minX, maxX, minY, maxY);
    }
}
