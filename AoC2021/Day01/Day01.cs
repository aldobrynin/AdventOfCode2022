namespace AoC2021.Day01;

public partial class Day01
{
    public static void Solve(IEnumerable<string> input)
    {
        var array = input.Select(int.Parse).ToArray();

        array.Skip(1)
            .Where((x, i) => x > array[i])
            .Count()
            .Part1();

        const int windowSize = 3;
        var windowedSums = array.SkipLast(windowSize - 1)
            .Select((_, i) => array.Skip(i).Take(windowSize).Sum())
            .ToArray();
        windowedSums.Skip(1)
            .Where((x, i) => x > windowedSums[i])
            .Count()
            .Part2();
    }
}

