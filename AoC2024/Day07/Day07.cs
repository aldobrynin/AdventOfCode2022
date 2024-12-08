namespace AoC2024.Day07;

public static partial class Day07 {
    public record Equation(long TestValue, long[] Arguments);

    public static void Solve(IEnumerable<string> input) {
        var equations = input.Select(x => x.ToLongArray(": "))
            .Select(x => new Equation(TestValue: x[0], Arguments: x[1..]))
            .ToArray();

        Solve2(useConcat: false).Part1();
        Solve2(useConcat: true).Part2();

        long Solve2(bool useConcat = false) =>
            equations
                .AsParallel()
                .Where(x => IsValid(x, useConcat))
                .Sum(x => x.TestValue);

        bool IsValid(Equation equation, bool useConcat) =>
            equation.Arguments.Length == 0
                ? equation.TestValue == 0
                : GetEquations(equation, useConcat).Any(eq => IsValid(eq, useConcat));

        IEnumerable<Equation> GetEquations(Equation equation, bool useConcat) {
            var last = equation.Arguments[^1];

            if (equation.TestValue >= last)
                yield return new Equation(equation.TestValue - last, equation.Arguments[..^1]);

            if (Math.DivRem(equation.TestValue, last) is (var divResult, Remainder: 0))
                yield return new Equation(divResult, equation.Arguments[..^1]);

            if (useConcat) {
                var digits = (long)Math.Pow(10, Math.Floor(Math.Log10(last) + 1));
                var (quotient, remainder) = Math.DivRem(equation.TestValue, digits);

                if (remainder == last)
                    yield return new Equation(quotient, equation.Arguments[..^1]);
            }
        }
    }
}
