using Common.AoC;
using Range = Common.Range;

namespace AoC2023.Day24;

public static partial class Day24 {
    public static void Solve(IEnumerable<string> input) {
        var hailstones = input.Select(line => line.Split(" @ "))
            .Select(x => new Hailstone(V3<double>.Parse(x[0]), V3<double>.Parse(x[1])))
            .ToArray();
        var range = AoCContext.IsSample
            ? Range.FromStartAndEndInclusive(7d, 27)
            : Range.FromStartAndEndInclusive(200000000000000d, 400000000000000);
        hailstones.Pairs().Count(x => HaveCrossPointInRange(x.A, x.B, range)).Part1();

        var (position, _) = Part2(hailstones);
        new[] { position.X, position.Y, position.Z }.Sum(long.CreateChecked).Part2();
    }

    private static bool HaveCrossPointInRange(Hailstone first, Hailstone second, Range<double> range) {
        List<V3<double>> vectors = [first.Velocity, -second.Velocity, second.Position - first.Position];
        var matrix = new[] {
            vectors.Select(v => new Rational((long)v.X)).ToArray(),
            vectors.Select(v => new Rational((long)v.Y)).ToArray(),
        };
        try {
            var times = matrix.SolveGaussian();
            // cross point in past
            if (times.Any(Rational.IsNegative)) return false;
            var crossPoint = first.Position + first.Velocity * times[0].ToDouble();
            return IsInRange(crossPoint);
        }
        catch {
            // no solution (parallel lines)
            return false;
        }

        bool IsInRange(V3<double> point) => range.Contains(point.X) && range.Contains(point.Y);
    }

    private static Hailstone Part2(Hailstone[] stones) {
        var (point1, time1) = FindCollisionTimeAndPoint(stones[0], stones[1], stones[2]);
        var (point2, time2) = FindCollisionTimeAndPoint(stones[0], stones[1], stones[3]);

        var velocity = (point2 - point1) / (time2 - time1);
        var point = point1 - velocity * time1;
        return new Hailstone(point, velocity);
    }

    private static (V3<double> Point, double Time) FindCollisionTimeAndPoint(Hailstone origin, Hailstone one,
        Hailstone two) {
        var (p1, v1) = one - origin;
        var (p2, v2) = two - origin;

        V3<double>[] vectors = [p1, v1, -v2, p2];
        var matrix = new Func<V3<double>, double>[] { x => x.X, x => x.Y, x => x.Z }
            .Select(c => vectors.Select(v => new Rational((long)c(v))).ToArray())
            .ToArray();

        var t = matrix.SolveGaussian()[^1];

        return (two.Position + two.Velocity * t.ToDouble(), t.ToDouble());
    }

    private record Hailstone(V3<double> Position, V3<double> Velocity) {
        public static Hailstone operator -(Hailstone a, Hailstone b) =>
            new(a.Position - b.Position, a.Velocity - b.Velocity);
    }
}