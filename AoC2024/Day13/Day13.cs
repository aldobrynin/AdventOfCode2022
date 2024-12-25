using System.Text.RegularExpressions;

namespace AoC2024.Day13;

public static partial class Day13 {
    public record Machine(V ButtonA, V ButtonB, V Prize) {
        public static Machine Parse(IReadOnlyList<string> lines) {
            var numberRegex = new Regex(@"\d+");
            var numbers = lines.Select(line => numberRegex.Matches(line).Select(x => x.Value.ToInt()).ToArray())
                .Select(x => new V(x[0], x[1]))
                .ToArray();
            return new Machine(numbers[0], numbers[1], numbers[2]);
        }
    }

    public static void Solve(IEnumerable<string> input) {
        var machines = input.SplitBy(string.IsNullOrWhiteSpace)
            .Select(Machine.Parse)
            .ToArray();

        machines.Select(CountTokensToPrize).Sum().Part1();

        var part2Shift = new V(10000000000000, 10000000000000);
        machines.Select(x => x with { Prize = x.Prize + part2Shift })
            .Select(CountTokensToPrize)
            .Sum()
            .Part2();
    }

    private static long CountTokensToPrize(Machine machine) {
        var (t1, t2) = SolveLinearEquation(machine.ButtonA, machine.ButtonB, machine.Prize);
        return 3 * t1 + t2;
    }

    // ax*t1 + bx*t2 = px
    // ay*t1 + by*t2 = py
    private static V SolveLinearEquation(V a, V b, V p) {
        var det = a.X * b.Y - a.Y * b.X;
        if (det == 0) return V.Zero;
        var (t1, rem1) = Math.DivRem(p.X * b.Y - p.Y * b.X, det);
        if (rem1 != 0) return V.Zero;
        var (t2, rem2) = Math.DivRem(a.X * p.Y - a.Y * p.X, det);
        if (rem2 != 0) return V.Zero;
        return new(t1, t2);
    }
}
