using Range = Common.Range;

namespace AoC2021.Day20;

public class Day20
{
    private static readonly Range2d NeighborsRange = new(new Range(-1, 1), new Range(-1, 1));

    public static void Solve(IEnumerable<string> input)
    {
        var array = input.ToArray();
        var algorithm = array[0];

        var rawMap = array.Skip(2)
            .Select(line => line.Select(c => c == '#').ToArray())
            .ToArray();
        var range = new Range2d(new Range(0, rawMap[0].Length - 1), new Range(0, rawMap.Length - 1));
        var map = range.All().Where(rawMap.Get).ToHashSet();

        EnhanceMany(algorithm, map, range, count: 2).Count.Dump("Part1: ");
        EnhanceMany(algorithm, map, range, count: 50).Count.Dump("Part2: ");
    }

    private static HashSet<V> EnhanceMany(string algorithm, HashSet<V> map, Range2d range, int count)
    {
        var toggle = algorithm[0] == '#';
        for (var i = 1; i <= count; i++)
        {
            var borderPixel = toggle ? i % 2 == 0 ? 1 : 0 : 0;
            map = Enhance(algorithm, map, range, borderPixel);
            range = range.Grow(1);
        }

        return map;
    }

    private static HashSet<V> Enhance(string algorithm, IReadOnlySet<V> map, Range2d range, int borderPixel)
    {
        return range.Grow(1)
            .All()
            .AsParallel()
            .Where(v => algorithm[GetAlgoIndex(v)] == '#')
            .ToHashSet();

        int GetPixel(V arg)
        {
            if (map.Contains(arg))
                return 1;
            return range.Contains(arg) ? 0 : borderPixel;
        }

        int GetAlgoIndex(V pixel)
        {
            return NeighborsRange.All()
                .Select(d => pixel + d)
                .Aggregate(0, (res, cur) => res * 2 + GetPixel(cur));
        }
    }
}