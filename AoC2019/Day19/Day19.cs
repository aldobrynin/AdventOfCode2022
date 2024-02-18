using Range = Common.Range;

namespace AoC2019.Day19;

public static partial class Day19 {
    public static void Solve(IEnumerable<string> input) {
        var program = input.Single().ToLongArray();
        var range = Range.FromStartAndEnd(0, 50);
        var points = Range2d.From(range, range)
            .All()
            .Where(IsBeam)
            .ToArray();
        points.Length.Part1();

        var diff = new V(99, -99);
        var current = points.First();
        while (current is { X: < 5000, Y: < 5000 }) {
            if (IsBeam(current + diff)) break;

            current += V.Down;
            if (!IsBeam(current)) current += V.Right;
        }

        (current.X * 10000 + current.Y + diff.Y).Part2();

        bool IsBeam(V v) => new IntCodeComputer(program, [v.X, v.Y]).GetNextOutput().GetAwaiter().GetResult() == 1;
    }
}