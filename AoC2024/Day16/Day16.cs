namespace AoC2024.Day16;

public static partial class Day16 {
    public record State(V Position, V Direction) {
        public State Forward() => this with { Position = Position + Direction };
        public State TurnCW() => this with { Direction = new V(Direction.Y, -Direction.X) };
        public State TurnCCW() => this with { Direction = new V(-Direction.Y, Direction.X) };
        public State TurnAround() => this with { Direction = -Direction };
    }

    public static void Solve(IEnumerable<string> input) {
        var map = Map.From(input);
        var start = map.FindFirst('S');
        var end = map.FindFirst('E');
        var initialState = new State(start, V.E);
        var endStates = V.Directions4.Select(dir => new State(end, dir)).ToArray();

        var startToEnd = FindMinDistances([initialState]);
        var bestDistance = endStates
            .Min(x => startToEnd.GetValueOrDefault(x, int.MaxValue))
            .Part1();

        var endToStart = FindMinDistances(endStates);

        startToEnd
            .Where(x => x.Value + endToStart[x.Key.TurnAround()] == bestDistance)
            .Select(x => x.Key.Position)
            .Distinct()
            .Count()
            .Part2();

        IReadOnlyDictionary<State, int> FindMinDistances(State[] initialStates) =>
            SearchHelpers
                .Dijkstra(GetNextState, initialStates)
                .GroupBy(x => x.State, x => x.Distance)
                .ToDictionary(x => x.Key, x => x.Min());

        IEnumerable<SearchPathItem<State>> GetNextState(SearchPathItem<State> current) {
            var (state, _, _) = current;
            if (map[state.Position + state.Direction] != '#') yield return current.Next(state.Forward());

            yield return current.Next(state.TurnCW(), 1000);
            yield return current.Next(state.TurnCCW(), 1000);
        }
    }
}
