namespace AoC2023.Day08;

public partial class Day08 {
    public static void Solve(IEnumerable<string> input) {
        var blocks = input.SplitBy(string.IsNullOrEmpty).ToArray();
        var directions = blocks.First().Single();
        var network = blocks.Last()
            .Select(line => line.Split(new[] { ' ', '=', '(', ')', ',' }, StringSplitOptions.RemoveEmptyEntries))
            .ToDictionary(x => x[0], x => (Left: x[1], Right: x[2]));

        CountMinSteps(start: "AAA", isEndPredicate: x => x == "ZZZ")
            .Part1();

        network.Keys.Where(x => x.EndsWith('A'))
            .Select(start => CountMinSteps(start, isEndPredicate: x => x.EndsWith('Z')))
            .Aggregate(1L, MathHelpers.Lcm)
            .Part2();

        long CountMinSteps(string start, Func<string, bool> isEndPredicate) =>
            SimulateSteps(start).TakeWhile(x => !isEndPredicate(x)).LongCount();

        IEnumerable<string> SimulateSteps(string start) {
            long steps = 0;
            var current = start;
            yield return current;
            while (true) {
                var direction = directions[(int)(steps++ % directions.Length)];
                var (left, right) = network[current];
                current = direction == 'L' ? left : right;
                yield return current;
            }
            // ReSharper disable once IteratorNeverReturns
        }
    }
}