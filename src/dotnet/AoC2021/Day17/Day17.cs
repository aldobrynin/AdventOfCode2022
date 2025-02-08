using System.Text.RegularExpressions;
using Range = Common.Range;

namespace AoC2021.Day17;

public partial class Day17 {
    public static void Solve(IEnumerable<string> input) {
        var numbers = Regex.Split(input.Single(), @"[^0-9\-]")
            .Where(x => !string.IsNullOrEmpty(x))
            .Select(int.Parse)
            .ToArray();
        var start = V.Zero;

        var xRange = Range.FromStartAndEndInclusive(numbers[0], numbers[1]);
        var yRange = Range.FromStartAndEndInclusive(numbers[2], numbers[3]);
        var rectangle = Range2d.From(xRange, yRange);

        var velocities = (rectangle.Y with { To = rectangle.Y.To + 500 })
            .SelectMany(y => 1L.RangeTo(rectangle.X.To).Select(x => new V(x, y)));

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

    private static IEnumerable<V> Simulate(V start, Range2d<int> target, V velocity) {
        var current = start;
        while (current.X < target.X.To && current.Y >= target.Y.From) {
            yield return current;
            if (target.Contains(current)) yield break;
            current += velocity;
            velocity = new V(velocity.X - Math.Sign(velocity.X), velocity.Y - 1);
        }
    }
}