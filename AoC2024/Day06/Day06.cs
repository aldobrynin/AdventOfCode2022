namespace AoC2024.Day06;

public static partial class Day06 {
    public record State(V Pos, V Dir) {
        public State TurnRight() => this with { Dir = Dir.Rotate(90) };
        public State Forward() => this with { Pos = Pos + Dir };
    }

    public static void Solve(IEnumerable<string> input) {
        var map = Map.From(input);
        var start = map.FindFirst(x => x is not '.' and not '#');
        var startDir = V.FromArrow(map[start]);
        var initialState = new State(start, startDir);

        var path = Walk(map, initialState)
            .Select(x => x?.Pos)
            .Distinct()
            .ToArray();

        path.Length.Part1();

        path.AsParallel()
            .Count(obstruction => Walk(map, initialState, obstruction).Last() is null)
            .Part2();
    }

    private static IEnumerable<State?> Walk(Map<char> map, State initialState, V? obstruction = null) {
        var visited = new HashSet<State> { initialState };
        var current = initialState;
        yield return current;

        while (Move(map, current, obstruction) is {} next) {
            if (visited.Add(next)) {
                yield return next;
                current = next;
            }
            else {
                yield return null;
                yield break;
            }
        }
    }


    private static State? Move(Map<char> map, State current, V? obstruction = null) {
        var next = current.Forward();
        while (next.Pos == obstruction || map.GetValueOrDefault(next.Pos) is '#') {
            current = current.TurnRight();
            next = current.Forward();
        }

        return next.Pos.IsInRange(map) ? next : null;
    }
}
