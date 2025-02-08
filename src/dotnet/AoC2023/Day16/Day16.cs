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
            var queue = new List<State> { initialState };
            var visited = queue.ToHashSet();
            var i = 0;
            while (i < queue.Count) {
                var state = queue[i++];
                queue.AddRange(GetNextStates(state).Where(next => map.Contains(next.Position) && visited.Add(next)));
            }

            return visited.DistinctBy(x => x.Position).Count();
        }

        IReadOnlyCollection<State> GetNextStates(State state) => (map[state.Position], state.Direction) switch {
            ('.', _) or ('-', { Y: 0 }) or ('|', { X: 0 }) => [state.Step()],
            ('-' or '|', _) => [state.TurnClockwise(), state.TurnCounterClockwise()],
            ('/', { Y: 0 }) => [state.TurnCounterClockwise()],
            ('/', { X: 0 }) => [state.TurnClockwise()],
            ('\\', { Y: 0 }) => [state.TurnClockwise()],
            ('\\', { X: 0 }) => [state.TurnCounterClockwise()],
            _ => throw new Exception($"Unexpected state: {state}"),
        };
    }

    private static State Step(this State state) => (state.Position + state.Direction, state.Direction);

    private static State TurnClockwise(this State state) {
        var direction = V.Directions4[(Array.IndexOf(V.Directions4, state.Direction) + 1).Mod(V.Directions4.Length)];
        return (state.Position + direction, direction);
    }

    private static State TurnCounterClockwise(this State state) {
        var direction = V.Directions4[(Array.IndexOf(V.Directions4, state.Direction) - 1).Mod(V.Directions4.Length)];
        return (state.Position + direction, direction);
    }
}