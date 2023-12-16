namespace AoC2023.Day16;
using State = (V Position, V Direction);

public static partial class Day16 {
    public static void Solve(IEnumerable<string> input) {
        var map = Map.From(input);

        CountEnergizedTiles(new State(V.Zero, V.Right)).Part1();

        var borders = new[] { map.TopBorder(), map.RightBorder(), map.BottomBorder(), map.LeftBorder() };
        var directions = new[] { V.Down, V.Left, V.Up, V.Right };
        borders
            .Zip(directions, (border, dir) => border.Select(v => (v, dir)))
            .Flatten()
            .AsParallel()
            .Max(CountEnergizedTiles)
            .Part2();

        int CountEnergizedTiles(State initialState) {
            var current = new List<State> { initialState };
            var visited = current.ToHashSet();
            while (current.Count > 0) current = current.SelectMany(GetNextStates).Where(visited.Add).ToList();
            return visited.DistinctBy(x => x.Position).Count();
        }

        IEnumerable<State> GetNextStates(State state) =>
            GetNextDirections(state).Select(dir => state.TurnAndMove(dir)).Where(s => map.Contains(s.Position));

        IEnumerable<V> GetNextDirections(State state) => (map[state.Position], state.Direction) switch {
            ('.', _) or ('-', { Y: 0 }) or ('|', { X: 0 }) => new[] { state.Direction },
            ('-' or '|', _) => new[] { state.Direction.Rotate(90), state.Direction.Rotate(270) },
            ('/', _) => new[] { state.Direction.Rotate(state.Direction.X * 180 + 90) },
            ('\\', _) => new[] { state.Direction.Rotate(state.Direction.Y * 180 + 90) },
            _ => throw new Exception($"Unexpected state: {state}"),
        };
    }

    private static State TurnAndMove(this State state, V direction) => (state.Position + direction, direction);
}