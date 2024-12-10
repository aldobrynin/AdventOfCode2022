namespace AoC2024.Day10;

public static partial class Day10 {
    public static void Solve(IEnumerable<string> input) {
        var map = Map.From(input);
        var heads = map.FindAll('0').ToArray();

        heads
            .Sum(head => map.Bfs(CanMove, head).Count(x => map[x.State] == '9'))
            .Part1();

        heads.Sum(CountTrails).Part2();

        int CountTrails(V head) {
            var queue = new Queue<V>([head]);
            var trailsToNine = 0;
            while (queue.TryDequeue(out var current)) {
                if (map[current] == '9') trailsToNine++;
                foreach (var next in map.Area4(current).Where(next => CanMove(current, next))) {
                    queue.Enqueue(next);
                }
            }

            return trailsToNine;
        }

        bool CanMove(V from, V to) => map[to] - map[from] == 1;
    }
}
