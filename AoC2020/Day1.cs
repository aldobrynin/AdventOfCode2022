using Common;

namespace AoC2020;

public class Day1
{
    public static void Solve(IEnumerable<string> input)
    {
        var d = new HashSet<long>();
        var array = input.Select(long.Parse).Order().ToArray();

        foreach (var n in array)
        {
            if (d.Contains(2020 - n))
            {
                (n * (2020 - n)).Dump("Part1: ");
                break;
            }

            d.Add(n);
        }


        Part2(array);
    }

    private static void Part2(long[] array)
    {
        for (var i = 0; i < array.Length; ++i)
        {
            var j = i + 1;
            var k = array.Length - 1;

            while (j < k)
            {
                var sum = array[i] + array[j] + array[k];
                if (sum == 2020)
                {
                    (array[i] * array[j] * array[k]).Dump("Part2: ");
                    return;
                }

                if (sum >= 2020) --k;
                else ++j;
            }
        }
    }
}