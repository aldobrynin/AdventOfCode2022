using System.Text.RegularExpressions;

namespace AoC2021.Day17;

public partial class Day17
{
    private class Rectangle
    {
        private readonly V _topLeft;
        private readonly V _rightBottom;

        public int MaxX => Math.Max(_topLeft.X, _rightBottom.X);
        public int MinX => Math.Min(_topLeft.X, _rightBottom.X);

        public int MaxY => Math.Max(_topLeft.Y, _rightBottom.Y);
        public int MinY => Math.Min(_topLeft.Y, _rightBottom.Y);

        public Rectangle(V topLeft, V rightBottom)
        {
            _topLeft = topLeft;
            _rightBottom = rightBottom;
        }

        public bool Contains(V v) => MinY <= v.Y && v.Y <= MaxY && MinX <= v.X && v.X <= MaxX;

        public override string ToString() => $"[x={MinX}..{MaxX}, y={MinY}..{MaxY}";
    }

    public static void Solve(IEnumerable<string> input)
    {
        var numbers = Regex.Split(input.Single(), @"[^0-9\-]")
            .Where(x => !string.IsNullOrEmpty(x))
            .Select(int.Parse)
            .ToArray();
        var rectangle = new Rectangle(
            new V(Math.Min(numbers[0], numbers[1]), Math.Max(numbers[2], numbers[3])),
            new V(Math.Max(numbers[0], numbers[1]), Math.Min(numbers[2], numbers[3]))
        );
        var start = V.Zero;

        var velocities = Enumerable.Range(rectangle.MinY, rectangle.MaxY + 500)
            .SelectMany(y => Enumerable.Range(1, rectangle.MaxX).Select(x => new V(x, y)));

        var launchResults = velocities
            .Select(v => (Velocity: v, Trajectory: Simulate(start, rectangle, v).ToArray()))
            .Where(x => rectangle.Contains(x.Trajectory.Last()))
            .ToArray();

        launchResults
            .Max(x => x.Trajectory.Max(t => t.Y))
            .Part1();

        launchResults
            .Distinct()
            .Count()
            .Part2();
    }

    private static IEnumerable<V> Simulate(V start, Rectangle target, V velocity)
    {
        var current = start;
        while (current.X <= target.MaxX && current.Y >= target.MinY)
        {
            yield return current;
            if (target.Contains(current))
                yield break;
            current += velocity;
            velocity = new V(velocity.X - Math.Sign(velocity.X), velocity.Y - 1);
        }
    }
}