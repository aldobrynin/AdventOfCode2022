namespace AoC2024.Day06;

public static partial class Day06 {
    public record State(V Pos, V Dir) {
        public State TurnRight() => this with { Dir = new V(Dir.Y, -Dir.X) };
        public State Forward() => this with { Pos = Pos + Dir };
    }

    public static void Solve(IEnumerable<string> input) {
        var map = Map.From(input);
        var start = map.FindFirst(x => x is not '.' and not '#');
        var startDir = V.FromArrow(map[start]);
        var initialState = new State(start, startDir);

        var path = Walk()
            .Select(x => x.Pos)
            .ToHashSet();

        path.Count.Part1();

        path.Count(HasCycle).Part2();

        IEnumerable<State> Walk(V? obstruction = null) {
            for (var state = initialState; map.Contains(state.Pos); state = Move(state, obstruction))
                yield return state;
        }

        State Move(State current, V? obstruction) {
            var next = current.Forward();
            while (next.Pos == obstruction || map.GetValueOrDefault(next.Pos) is '#') {
                current = current.TurnRight();
                next = current.Forward();
            }

            return next;
        }

        bool HasCycle(V obstruction) {
            const int cyclePathStepsThreshold = 10_000;
            return Walk(obstruction).Skip(cyclePathStepsThreshold).Any();
        }

    }
}
