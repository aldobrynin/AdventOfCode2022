namespace AoC2019.Day15;

public static partial class Day15 {
    public static void Solve(IEnumerable<string> input) {
        var program = input.Single().ToLongArray();

        var map = BuildMap(program);
        var (oxygen, distance, _) = SearchHelpers
            .Bfs(state => state.Area4().Where(v => map[v] != '#'), initialStates: V.Zero)
            .First(x => map[x.State] == 'O');
        distance.Part1();

        SearchHelpers
            .Bfs(state => state.Area4().Where(v => map[v] != '#'), initialStates: oxygen)
            .Max(x => x.Distance)
            .Part2();
    }

    private static Dictionary<V, char> BuildMap(long[] program) {
        var computer = new IntCodeComputer(program);
        var start = V.Zero;
        var current = SearchPathItem.Start(start);
        var visited = new HashSet<V> { start };
        var map = new Dictionary<V, char> {
            [start] = 'D'
        };

        while (current != null) {
            var next = current.State.Area4().FirstOrDefault(visited.Add);
            if (next is not null) {
                if (MoveTo(next)) current = current.Next(next);
            }
            else {
                if (current.Prev != null) MoveTo(current.Prev.State);
                current = current.Prev;
            }
        }

        return map;

        bool MoveTo(V nextPos) {
            computer.AddInput(DirectionToInput(nextPos - current.State));
            var output = computer.GetNextOutput().GetAwaiter().GetResult();
            map[nextPos] = output switch {
                0 => '#',
                1 => '.',
                2 => 'O',
                _ => throw new Exception($"Unexpected output '{output}'")
            };
            return output != 0;
        }
    }

    private static long DirectionToInput(V direction) {
        if (direction == V.N) return 1;
        if (direction == V.S) return 2;
        if (direction == V.W) return 3;
        if (direction == V.E) return 4;
        throw new ArgumentOutOfRangeException(nameof(direction), direction, "Unexpected direction");
    }
}