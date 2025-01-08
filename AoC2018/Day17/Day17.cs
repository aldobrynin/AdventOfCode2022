using Range = Common.Range;

namespace AoC2018.Day17;

public static partial class Day17 {
    public static void Solve(IEnumerable<string> input) {
        var clay = input.SelectMany(line => {
            var parts = line.Split([',', ' '], StringSplitOptions.RemoveEmptyEntries);
            var xRange = ParseRange(parts.First(x => x.StartsWith('x')).Split('=')[^1]);
            var yRange = ParseRange(parts.First(x => x.StartsWith('y')).Split('=')[^1]);
            return Range2d.From(xRange, yRange).All();
        })
        .ToHashSet();

        Simulate(clay);
        Range<int> ParseRange(string s) {
            var parts = s.ToIntArray(".");
            return Range.FromStartAndEndInclusive(parts.First(), parts.Last());
        }
    }

    private static void Simulate(HashSet<V> clay) {
        var (minY, maxY) = clay.Select(v => v.Y).MinMax();

        var source = new V(500, 0);
        var runningWater = new HashSet<V>();
        var settledWater = new HashSet<V>();

        DropWater(source);

        (runningWater.Count + settledWater.Count).Part1();
        settledWater.Count.Part2();

        void DropWater(V from) {
            if (from.Y > maxY) return;
            if (from.Y >= minY) runningWater.Add(from);

            var down = from + V.Down;
            if (IsEmpty(down)) DropWater(down);
            if (IsClayOrSettled(down) && IsEmpty(from + V.Left)) DropWater(from + V.Left);
            if (IsClayOrSettled(down) && IsEmpty(from + V.Right)) DropWater(from + V.Right);

            var leftBoundary = from
                .GenerateSequence(v => v + V.Left)
                .TakeUntil(v => !runningWater.Contains(v))
                .ToArray();
            if (!clay.Contains(leftBoundary[^1])) return;

            var rightBoundary = from
                .GenerateSequence(v => v + V.Right)
                .TakeUntil(v => !runningWater.Contains(v))
                .ToArray();
            if (!clay.Contains(rightBoundary[^1])) return;

            foreach (var v in leftBoundary.Where(runningWater.Remove)) settledWater.Add(v);
            foreach (var v in rightBoundary.Where(runningWater.Remove)) settledWater.Add(v);
        }

        bool IsClayOrSettled(V v) => clay.Contains(v) || settledWater.Contains(v);
        bool IsEmpty(V v) => !clay.Contains(v) && !runningWater.Contains(v) && !settledWater.Contains(v);
    }
}
