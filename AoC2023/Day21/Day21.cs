using Common.AoC;
namespace AoC2023.Day21;

public static partial class Day21 {
    public static void Solve(IEnumerable<string> input) {
        var map = Map.From(input);

        CountVisitedTiles(AoCContext.IsSample ? 6 : 64, map).Last().Part1();

        if (!AoCContext.IsSample) Part2(26501365L, map).Part2();
    }


    private static long Part2(long targetSteps, Map<char> map) {
        var (grids, rem) = Math.DivRem(targetSteps, map.SizeX).Dump();

        var stepsToSimulate = 2 * map.SizeX + rem;
        stepsToSimulate.Dump("Steps to simulate: ");

        var sequence = CountVisitedTiles(stepsToSimulate, map)
            .Where((_, step) => step % map.SizeX == rem)
            .Order()
            .ToArray();

        var (a, b, c) = FindQuadraticEquationCoefficients(sequence[0], sequence[1], sequence[2]);
        return a * grids * grids + b * grids + c;
    }

    // f(x) = a*x^2 + b*x + c
    // f(0) = a*0 + b*0 + c = f0
    // f(1) = a*1 + b*1 + c = f1
    // f(2) = a*4 + b*2 + c = f2
    // c = f0
    // a = (f2 - 2*f1 + c)/2
    // b = f1 - c - a
    private static (long A, long B, long C) FindQuadraticEquationCoefficients(long f0, long f1, long f2) {
        var c = f0;
        var a = (f2 - 2 * f1 + c) / 2;
        var b = f1 - c - a;
        return (a, b, c);
    }

    private static IEnumerable<long> CountVisitedTiles(long steps, Map<char> map) {
        var current = map.FindAll('S').ToHashSet();
        yield return current.Count;
        foreach (var _ in 1L.RangeTo(steps)) {
            current = current
                .SelectMany(v => v.Area4().Where(v1 => map[v1.Mod(map.SizeY, map.SizeX)] != '#'))
                .ToHashSet();
            yield return current.Count;
        }
    }
}