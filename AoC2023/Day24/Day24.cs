using Common.AoC;

namespace AoC2023.Day24;

public static partial class Day24 {
    public static void Solve(IEnumerable<string> input) {
        var hailstones = input.Select(line => line.Split(" @ "))
            .Select(x => new Hailstone(Point3D.Parse(x[0]), Point3D.Parse(x[1])))
            .ToArray();
        var min = AoCContext.IsSample ? 7 : 200000000000000;
        var max = AoCContext.IsSample ? 27 : 400000000000000;
        hailstones.Pairs().Count(x => x.A.GetCrossPoint(x.B) is { } point && IsInRange(point)).Part1();

        bool IsInRange((double, double) point) {
            var (x, y) = point;
            return x >= min && x <= max && y >= min && y <= max;
        }
    }

    private record Point3D(double X, double Y, double Z) {
        public static Point3D Parse(string s) {
            var components = s.Split(new[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries);
            return new Point3D(double.Parse(components[0]), double.Parse(components[1]), double.Parse(components[2]));
        }
    }

    private record Hailstone(Point3D Position, Point3D Velocity) {
        public (double, double)? GetCrossPoint(Hailstone other) {
            var (a1, c1) = GetCoefficients();
            var (a2, c2) = other.GetCoefficients();
            var x = (c2 - c1) / (a1 - a2);
            var y = a1 * x + c1;

            if (GetTimeAtPoint(x) < 0 || other.GetTimeAtPoint(x) < 0) return null;
            return (x, y);
        }

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