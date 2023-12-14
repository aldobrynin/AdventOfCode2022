namespace AoC2023.Day14;

public static partial class Day14 {

    public static void Solve(IEnumerable<string> input) {
        var map = Map.From(input);

        var dirs = new[] { V.N, V.W, V.S, V.E };
        var visitOrder = new Dictionary<V, V[]> {
            [V.N] = map.Coordinates().OrderBy(v => v.Y).ToArray(),
            [V.W] = map.Coordinates().OrderBy(v => v.X).ToArray(),
            [V.S] = map.Coordinates().OrderByDescending(v => v.Y).ToArray(),
            [V.E] = map.Coordinates().OrderByDescending(v => v.X).ToArray(),
        };

        TiltPlatform(V.N);
        map.GetLoad().Part1();

        const int cycles = 1_000_000_000;
        var loadsPerCycle = new List<int>();
        var simulationSequence = RunSimulation()
            .Pipe(x => loadsPerCycle.Add(x.Load))
            .Select(item => item.StateKey);
        var (cycleStart, period) = DetectCycle(simulationSequence);
        var remainder = (cycles - cycleStart) % period;
        loadsPerCycle[cycleStart + remainder].Part2();

        void TiltPlatform(V direction) {
            foreach (var coordinate in visitOrder[direction].Where(v => map[v] == 'O')) {
                var target = AllFromPointTo(coordinate, direction)
                    .TakeWhile(v => map.GetValueOrDefault(v, '#') == '.')
                    .LastOrDefault();

                if (target == null) continue;
                (map[target], map[coordinate]) = (map[coordinate], map[target]);
            }
        }

        IEnumerable<(int StateKey, int Load)> RunSimulation(int limit = int.MaxValue) {
            while (limit-- > 0) {
                yield return (map.GetHashCode(), map.GetLoad());
                foreach (var dir in dirs) TiltPlatform(dir);
            }
        }
    }

    private static (int CycleStart, int Period) DetectCycle(IEnumerable<int> sequence) {
        var visited = new Dictionary<int, int>();
        foreach (var (item, index) in sequence.WithIndex()) {
            if (!visited.TryGetValue(item, out var prevIndex)) {
                visited[item] = index;
                continue;
            }

            return (prevIndex, index - prevIndex);
        }

        throw new Exception("No solution found");
    }

    private static IEnumerable<V> AllFromPointTo(V start, V dir, int limit = int.MaxValue) {
        var current = start + dir;
        for (var i = 0; i < limit; i++, current += dir)
            yield return current;
    }

    private static int GetLoad(this Map<char> map) => map.FindAll('O').Sum(v => map.SizeY - v.Y);
}