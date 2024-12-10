namespace AoC2024.Day10;

public static partial class Day10 {
    public static void Solve(IEnumerable<string> input) {
        var map = Map.From(input);

        var trailEnds = map
            .FindAll('0')
            .SelectMany(head => VisitAll(head).Where(v => map[v] == '9').CountBy(x => x))
            .ToArray();

        trailEnds.Length.Part1();
        trailEnds.Sum(x => x.Value).Part2();

        IEnumerable<V> VisitAll(V start) {
            var queue = new Queue<V>([start]);
            while (queue.TryDequeue(out var current)) {
                yield return current;
                foreach (var next in map.Area4(current).Where(next => map[next] - map[current] == 1)) {
                    queue.Enqueue(next);
                }
            }
        }
    }
}
