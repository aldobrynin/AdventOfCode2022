namespace AoC2020.Day17;

public class Day17 {
    private const char Active = '#';

    public static void Solve(IEnumerable<string> input) {
        var active = input
            .SelectMany((line, yIndex) => line.Select((c, xIndex) => (State: c, new V4(xIndex, yIndex, 1, 1))))
            .Where(x => x.State == Active)
            .Select(x => x.Item2)
            .ToHashSet();

        Emulate(active, v => v.Neighbors3())
            .ElementAt(6)
            .Dump("Part1: ");

        Emulate(active, v => v.Neighbors4())
            .ElementAt(6)
            .Dump("Part2: ");
    }

    private static IEnumerable<int> Emulate(HashSet<V4> active, Func<V4, IEnumerable<V4>> getNeighbors) {
        while (active.Count > 0) {
            yield return active.Count;
            active = active
                .SelectMany(getNeighbors)
                .GroupBy(x => x)
                .Select(x => (x.Key, Count: x.Count()))
                .Where(x => x.Count == 3 || x.Count == 2 && active.Contains(x.Key))
                .Select(x => x.Key)
                .ToHashSet();
        }
    }

    public readonly record struct V4(int X, int Y, int Z, int W) {
        public IEnumerable<V4> Neighbors4() {
            for (var dx = -1; dx <= 1; dx++)
            for (var dy = -1; dy <= 1; dy++)
            for (var dz = -1; dz <= 1; dz++)
            for (var dw = -1; dw <= 1; dw++)
                if (dx != 0 || dy != 0 || dz != 0 || dw != 0)
                    yield return new V4(X + dx, Y + dy, Z + dz, W + dw);
        }

        public IEnumerable<V4> Neighbors3() {
            for (var dx = -1; dx <= 1; dx++)
            for (var dy = -1; dy <= 1; dy++)
            for (var dz = -1; dz <= 1; dz++)
                if (dx != 0 || dy != 0 || dz != 0)
                    yield return new V4(X + dx, Y + dy, Z + dz, W);
        }
    }
}