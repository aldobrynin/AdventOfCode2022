namespace AoC2021.Day24;

public partial class Day24 {
    public static void Solve(IEnumerable<string> input) {
        var array = input.ToArray();
        Run(array).Part1();
        Run(array, min: true).Part2();
    }

    private static long Run(IEnumerable<string> input, bool min = false) {
        var numbers = Enumerable.Range(1, 9).ToArray();

        var result = new int[14];
        var stack = new Stack<(int, int)>();
        var args = input
            .Chunk(18)
            .Select(chunk => (X: int.Parse(chunk[5].Split(' ')[2]), Y: int.Parse(chunk[15].Split(' ')[2])))
            .ToArray();
        for (var i = 0; i < 14; i++) {
            var (xAdd, yAdd) = args[i];
            if (xAdd > 0) {
                stack.Push((yAdd, i));
                continue;
            }

            (yAdd, var yIndex) = stack.Pop();
            result[yIndex] = min
                ? numbers.First(x => x + yAdd + xAdd >= 1)
                : numbers.Reverse().First(x => x + yAdd + xAdd <= 9);

            result[i] = result[yIndex] + yAdd + xAdd;
        }

        return result.Aggregate(0L, (cur, d) => cur * 10 + d);
    }
}

