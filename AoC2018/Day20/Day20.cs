namespace AoC2018.Day20;

public static partial class Day20 {
    public static void Solve(IEnumerable<string> input) {
        var pattern = input.First();

        var directions = new Dictionary<char, V> {
            ['N'] = V.N,
            ['E'] = V.E,
            ['S'] = V.S,
            ['W'] = V.W,
        };
        var current = V.Zero;
        var rooms = new Dictionary<V, int>();
        var stack = new Stack<V>();
        foreach (var c in pattern) {
            if (c == '(') stack.Push(current);
            else if (c == ')') current = stack.Pop();
            else if (c == '|') current = stack.Peek();
            else if (directions.TryGetValue(c, out var dir)) {
                var distance = rooms.GetValueOrDefault(current) + 1;
                current += dir;
                rooms[current] = Math.Min(rooms.GetValueOrDefault(current, int.MaxValue), distance);
            }
        }

        rooms.Max(r => r.Value).Part1();
        rooms.Count(r => r.Value >= 1000).Part2();
    }
}
