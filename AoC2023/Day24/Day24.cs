using Common.AoC;

namespace AoC2023.Day24;

public static partial class Day24 {
    public static void Solve(IEnumerable<string> input) {
        var hailstones = input.Select(line => line.Split(" @ "))
            .Select(x => new Hailstone(V3<double>.Parse(x[0]), V3<double>.Parse(x[1])))
            .ToArray();
        var min = AoCContext.IsSample ? 7 : 200000000000000;
        var max = AoCContext.IsSample ? 27 : 400000000000000;
        hailstones.Pairs().Count(x => x.A.GetCrossPoint(x.B) is { } point && IsInRange(point)).Part1();

        var (position, _) = Part2(hailstones);
        ((long)(position.X + position.Y + position.Z)).Part2();

        bool IsInRange((double, double) point) {
            var (x, y) = point;
            return x >= min && x <= max && y >= min && y <= max;
        }
    }

    private static Hailstone Part2(Hailstone[] stones) {
        var (point1, time1) = FindCollisionTimeAndPoint(stones[0], stones[1], stones[2]);
        var (point2, time2) = FindCollisionTimeAndPoint(stones[0], stones[1], stones[3]);

        var velocity = (point2 - point1) / (time2 - time1);
        var point = point1 - velocity * time1;
        return new Hailstone(point, velocity);
    }

    static (V3<double> Point, double Time) FindCollisionTimeAndPoint(Hailstone origin, Hailstone one, Hailstone two) {
        var (p1, v1) = one - origin;
        var (p2, v2) = two - origin;

        V3<double>[] vectors = [p1, v1, -v2, p2];
        var matrix = new Func<V3<double>, double>[] { x => x.X, x => x.Y, x => x.Z, }
            .Select(coordinate => vectors.Select(vector => new Rational((long)coordinate(vector))).ToArray())
            .ToArray();

        var t = Gaussian(matrix)[^1];
        return (two.Position + two.Velocity * t.ToDouble(), t.ToDouble());
    }

    private static Rational[] Gaussian(Rational[][] matrix) {
        var m = matrix.Length;
        for (var k = 0; k < m; k++) {
            var maxRow = k;
            for (var j = k + 1; j < m; j++)
                if (Rational.Abs(matrix[j][k]) > Rational.Abs(matrix[maxRow][k]))
                    maxRow = j;

            (matrix[maxRow], matrix[k]) = (matrix[k], matrix[maxRow]);

            for (var i = k + 1; i < m; i++) {
                var factor = matrix[i][k] / matrix[k][k];
                for (var j = k + 1; j <= m; j++) matrix[i][j] -= factor * matrix[k][j];
                matrix[i][k] = Rational.Zero;
            }
        }

        var result = new Rational[m];
        for (var i = m - 1; i >= 0; i--) {
            result[i] = matrix[i][m];
            for (var j = i + 1; j < m; j++) result[i] -= result[j] * matrix[i][j];
            result[i] /= matrix[i][i];
        }

        return result;
    }

    private record Hailstone(V3<double> Position, V3<double> Velocity) {
        public (double, double)? GetCrossPoint(Hailstone other) {
            var (a1, c1) = GetCoefficients();
            var (a2, c2) = other.GetCoefficients();
            var x = (c2 - c1) / (a1 - a2);
            var y = a1 * x + c1;

            if (GetTimeAtPoint(x) < 0 || other.GetTimeAtPoint(x) < 0) return null;
            return (x, y);
        }

        public static Hailstone operator -(Hailstone a, Hailstone b) =>
            new(a.Position - b.Position, a.Velocity - b.Velocity);

        private double GetTimeAtPoint(double x) => (x - Position.X) / Velocity.X;

        private (double A, double C) GetCoefficients() {
            // x = pos_x + vx * t
            // y = pos_y + vy * t
            // y = pos_y - vx * pos_x / vy + vx / vy * x
            // a = vx / vy, c = pos_y - vx * pos_x / vy
            var a = Velocity.Y / Velocity.X;
            var c = Position.Y - a * Position.X;
            return (a, c);
        }
    }
}