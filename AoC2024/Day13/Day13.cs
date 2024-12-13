using System.Text.RegularExpressions;

namespace AoC2024.Day13;

public static partial class Day13 {
    public record Machine(V ButtonA, V ButtonB, V Prize) {
        public static Machine Parse(IReadOnlyList<string> lines) {
            return new Machine(ParseButton(lines[0]), ParseButton(lines[1]), ParsePrize(lines[2]));
        }

        private static V ParseButton(string input) {
            var match = Regex.Match(input, @"X\+(\d+), Y\+(\d+)");
            return new V(int.Parse(match.Groups[1].Value), int.Parse(match.Groups[2].Value));
        }

        private static V ParsePrize(string input) {
            var match = Regex.Match(input, @"X=(\d+), Y=(\d+)");
            return new V(int.Parse(match.Groups[1].Value), int.Parse(match.Groups[2].Value));
        }
    }

    public static void Solve(IEnumerable<string> input) {
        var machines = input.SplitBy(string.IsNullOrWhiteSpace)
            .Select(Machine.Parse)
            .ToArray();

        machines.Select(Solve).Sum().Part1();

        var part2Shift = new V(10000000000000, 10000000000000);
        machines.Select(x => x with { Prize = x.Prize + part2Shift })
            .Select(Solve)
            .Sum()
            .Part2();
    }

    private static long Solve(Machine machine) {
        var matrix = new Rational[][] {
            [new(machine.ButtonA.X), new Rational(machine.ButtonB.X), new Rational(machine.Prize.X)],
            [new(machine.ButtonA.Y), new Rational(machine.ButtonB.Y), new Rational(machine.Prize.Y)],
        };
        var solution = matrix.SolveGaussian();
        if (solution.Any(x => x.Denominator != 1)) return 0;
        return (long)(solution[0].Numerator * 3 + solution[1].Numerator);
    }
}
