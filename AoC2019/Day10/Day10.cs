namespace AoC2019.Day10;

public static partial class Day10 {
    public static void Solve(IEnumerable<string> input) {
        var map = Map.From(input);
        var asteroids = map.FindAll('#').ToArray();
        var (maxPosition, maxVisibleCount) = asteroids
            .Select(asteroid => (Pos: asteroid, Count: asteroids.Count(other => IsVisible(map, asteroid, other))))
            .MaxBy(x => x.Count);
        maxVisibleCount.Part1();
        Vaporize(maxPosition).Select(v => v.X * 100 + v.Y).ElementAtOrDefault(199).Part2();

        IEnumerable<V> Vaporize(V location) {
            var candidates = asteroids.Where(x => x != location)
                .Select(asteroid => (Asteroid: asteroid, Angle: Angle(asteroid - location)))
                .OrderBy(x => x.Angle)
                .ThenBy(x => (x.Asteroid - location).MLen)
                .ToList();
            (V Asteroid, double Angle)? prevTarget = null;
            while (candidates.Count > 0) {
                var prevAngle = prevTarget?.Angle ?? -1;
                var index = candidates.FindIndex(v => v.Angle > prevAngle);
                if (index == -1) index = 0;
                prevTarget = candidates[index];
                candidates.RemoveAt(index);
                yield return prevTarget.Value.Asteroid;
            }
        }
    }

    private static double Angle(V asteroid) {
        var angle = Math.Atan2(asteroid.X, -asteroid.Y);
        if (angle < 0) angle += 2 * Math.PI;
        angle *= 180 / Math.PI;
        return angle;
    }

    private static bool IsVisible(this Map<char> map, V asteroid, V other) {
        var diff = other - asteroid;
        if (diff == V.Zero) return false;
        var gcd = MathHelpers.Gcd(diff.Abs().X, diff.Abs().Y);
        var step = diff / gcd;
        var current = asteroid + step;
        while (current != other && map[current] != '#') current += step;

        return current == other;
    }
}