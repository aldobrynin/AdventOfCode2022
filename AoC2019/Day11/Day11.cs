using Range = Common.Range;

namespace AoC2019.Day11;

public static partial class Day11 {
    public static void Solve(IEnumerable<string> input) {
        var program = input.Single().ToLongArray();
        Run(program, 0).Count.Part1();

        var painted = Run(program, 1);

        var xRange = Range.FromStartAndEndInclusive(painted.Min(x => x.Key.X), painted.Max(x => x.Key.X));
        var yRange = Range.FromStartAndEndInclusive(painted.Min(x => x.Key.Y), painted.Max(x => x.Key.Y));

        yRange.Select(y =>
                xRange.Select(x => new V(x, y))
                    .Select(v => painted.GetValueOrDefault(v) == 1 ? '█' : ' ')
                    .StringJoin(string.Empty))
            .Prepend(string.Empty)
            .StringJoin("\n")
            .Dump("Part2: ");
    }

    private static Dictionary<V, int> Run(long[] program, int startColor) {
        var current = V.Zero;
        var dir = V.Up;
        var visited = new Dictionary<V, int> { [current] = startColor };
        var computer = new IntCodeComputer(program);
        while (ReadNext(out var instruction)) {
            visited[current] = instruction.Color;
            dir = dir.Rotate(instruction.Turn == 0 ? -90 : 90);
            current += dir;
        }

        return visited;

        bool ReadNext(out (int Color, int Turn) instruction) {
            instruction = (-1, -1);
            computer.AddInput(visited.GetValueOrDefault(current));
            if (!computer.RunToNextOutput()) return false;
            instruction = ((int)computer.Output, (int)computer.GetNextOutput());
            return true;
        }
    }
}