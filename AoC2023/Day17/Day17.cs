namespace AoC2023.Day17;

public static partial class Day17 {
    public record State(V Position, V Dir, int Steps) {
        public State Next(V dir) => new(Position + dir, dir, dir == Dir ? Steps + 1 : 1);
    }

    public static void Solve(IEnumerable<string> input) {
        var map = Map.From(input.Select(line => line.Select(c => c - '0')));
        var initialState = new State(V.Zero, V.Zero, int.MaxValue);
        var end = new V(map.LastColumnIndex, map.LastRowIndex);

        FindShortestDistance(minSteps: 1, maxSteps: 3).Part1();
        FindShortestDistance(minSteps: 4, maxSteps: 10).Part2();

        int FindShortestDistance(int minSteps, int maxSteps) {
            return SearchHelpers.Dijkstra(GetNextStates, initialStates: initialState)
                .First(x => x.State.Steps >= minSteps && x.State.Position == end)
                .Distance;

            IEnumerable<SearchPathItem<State>> GetNextStates(SearchPathItem<State> current) =>
                V.Directions4
                    .Where(dir => dir != -current.State.Dir)
                    .Select(dir => current.State.Next(dir))
                    .Where(s => map.Contains(s.Position) && s.Steps <= maxSteps)
                    .Where(s => s.Dir == current.State.Dir || current.State.Steps >= minSteps)
                    .Select(s => current.Next(s, distanceCost: map[s.Position]));
        }
    }
}