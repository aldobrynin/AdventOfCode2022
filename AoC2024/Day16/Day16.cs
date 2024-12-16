namespace AoC2024.Day16;

public static partial class Day16 {
    public record State(V Position, V Direction) {
        public State Forward() => this with { Position = Position + Direction };
        public State TurnCW() => this with { Direction = new V(Direction.Y, -Direction.X) };
        public State TurnCCW() => this with { Direction = new V(-Direction.Y, Direction.X) };
    }

    public static void Solve(IEnumerable<string> input) {
        var map = Map.From(input);
        var start = map.FindFirst('S');
        var end = map.FindFirst('E');
        var initialState = new State(start, V.E);

        var bestDistance = SearchHelpers.Dijkstra(GetNextState, initialState)
            .First(x => x.State.Position == end)
            .Distance
            .Part1();

        var tiles = new HashSet<V>();
        var queue = new PriorityQueue<SearchPathItem<State>, int>();
        queue.Enqueue(SearchPathItem.Start(initialState), 0);
        var visited = new Dictionary<State, int>();
        while (queue.TryDequeue(out var current, out _)) {
            if (current.State.Position == end) {
                tiles.UnionWith(current.BackToStart().Select(x => x.Position));
                continue;
            }

            foreach (var next in GetNextState(current)) {
                if (next.Distance <= bestDistance && visited.GetValueOrDefault(next.State, int.MaxValue) >= next.Distance) {
                    visited[next.State] = next.Distance;
                    queue.Enqueue(next, next.Distance);
                }
            }
        }

        tiles.Count.Part2();

        IEnumerable<SearchPathItem<State>> GetNextState(SearchPathItem<State> current) {
            var (state, _, _) = current;
            if (map[state.Position + state.Direction] != '#')
                yield return current.Next(state.Forward());

            yield return current.Next(state.TurnCW(), 1000);
            yield return current.Next(state.TurnCCW(), 1000);
        }
    }
}
