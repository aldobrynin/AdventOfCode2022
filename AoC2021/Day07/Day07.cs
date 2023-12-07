namespace AoC2021.Day07;

public class Day7
{
    public static void Solve(IEnumerable<string> input)
    {
        var positions = input.Single().Split(',').Select(int.Parse).ToArray();
        Cost(positions, Cost).Sum.Dump("Part1: ");
        Cost(positions, Cost2).Sum.Dump("Part2: ");
    }

    private static (int Position, long Sum) Cost(int[] positions, Func<int, int, int> costFunc)
    {
        return Enumerable.Range(positions.Min(), positions.Max() - positions.Min())
            .Select(x => (Position: x, Sum: positions.Sum(p => costFunc(p, x))))
            .MinBy(x => x.Sum);
    }

    private static int Cost(int from, int to) => Math.Abs(from - to);

    private static int Cost2(int from, int to) => Math.Abs(to - from) * (Math.Abs(to - from) + 1) / 2;
}