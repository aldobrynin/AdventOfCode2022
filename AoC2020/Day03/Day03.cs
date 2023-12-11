namespace AoC2020.Day03;

public partial class Day03
{
    public static void Solve(IEnumerable<string> input)
    {
        var arr = input.Select(line => line.ToCharArray()).ToArray();
        var map = Map.From(arr);
        var start = V.Zero;

        CountTrees(start, V.Right * 3 + V.Up).Part1();

        new[]
            {
                V.Right + V.Up,
                3 * V.Right + V.Up,
                5 * V.Right + V.Up,
                7 * V.Right + V.Up,
                V.Right + 2 * V.Up,
            }
            .Select(m => CountTrees(start, m))
            .Product()
            .Part2();

        long CountTrees(V from, V s)
        {
            return Enumerate(from, s)
                .Select(v => v with { X = v.X.Mod(map.SizeX) })
                .TakeWhile(v => v.Y < map.SizeY)
                .LongCount(v => map[v] == '#');
        }
    }

    private static IEnumerable<V> Enumerate(V start, V move)
    {
        while (true)
        {
            yield return start;
            start += move;
        }
    }
}