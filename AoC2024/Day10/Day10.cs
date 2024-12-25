namespace AoC2024.Day10;

public static partial class Day10 {
    public static void Solve(IEnumerable<string> input) {
        var map = Map.From(input);
        var heads = map.FindAll('0').ToArray();

        heads
            .Sum(head => map.Bfs(CanMove, head).Count(x => map[x.State] == '9'))
            .Part1();

        var counts = CountTrails(heads);
        map.FindAll('9')
            .Sum(counts.GetValueOrDefault)
            .Part2();

        Dictionary<V, int> CountTrails(params V[] start) {
            var queue = new Queue<V>(start);
            var visits = start.ToDictionary(x => x, _ => 1);
            while (queue.TryDequeue(out var current)) {
                foreach (var next in map.Area4(current).Where(next => CanMove(current, next))) {
                    if (!visits.TryGetValue(next, out var nextCount)) queue.Enqueue(next);

                    visits[next] = nextCount + visits[current];
                }
            }
            return visits;
        }

        bool CanMove(V from, V to) => map[to] - map[from] == 1;
    }
}
