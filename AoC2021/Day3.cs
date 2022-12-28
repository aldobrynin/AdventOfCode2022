using Common;

namespace AoC2021;

public class Day3
{
    public static void Solve(IEnumerable<string> input)
    {
        var array = input.ToArray();
        var gamma = new char[array[0].Length];
        var epsilon = new char[array[0].Length];
        for (var i = 0; i < array[0].Length; i++)
        {
            var bits = array.Select(x => x[i])
                .GroupBy(x => x)
                .Select(x => (x.Key, Count: x.Count()))
                .OrderByDescending(x => x.Count)
                .Select(x => x.Key)
                .ToArray();
            gamma[i] = bits[0] ;
            epsilon[i] = bits[1];
        }

        (Convert.ToInt32(new string(gamma), 2)
         *
         Convert.ToInt32(new string(epsilon), 2))
            .Dump("Part1: ");

        (FindOxygenGeneratorRating(array) * FindCO2ScrubberRating(array))
            .Dump("Part2: ");
    }

    private static int FindOxygenGeneratorRating(string[] input)
    {
        var target = input;
        var bitIndex = 0;
        while (target.Length > 1) target = Filter(target, bitIndex++, true).ToArray();
        return target.Single().Select(x => x - '0').Aggregate(0, (cur, bit) => cur * 2 + bit);
    }
    
    private static int FindCO2ScrubberRating(string[] input)
    {
        var target = input;
        var bitIndex = 0;
        while (target.Length > 1) target = Filter(target, bitIndex++, false).ToArray();
        return target.Single().Select(x => x - '0').Aggregate(0, (cur, bit) => cur * 2 + bit);
    }

    private static IEnumerable<string> Filter(string[] input, int bitIndex, bool chooseGreater)
    {
        var first = input.Select(x => x[bitIndex]).ToArray();
        var sortedByFrequency = first.GroupBy(x => x)
            .Select(x => (x.Key, Count: x.Count()))
            .OrderByDescending(x => x.Count)
            .ThenByDescending(x => x.Key - '0')
            .Select(x => x.Key)
            .ToArray();
        var winnerBit = chooseGreater ? sortedByFrequency.First() : sortedByFrequency.Last();
        return input.Where(x => x[bitIndex] == winnerBit);
    }
}

