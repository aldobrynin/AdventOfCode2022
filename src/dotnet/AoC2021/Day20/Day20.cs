using Range = Common.Range;

namespace AoC2021.Day20;

public partial class Day20 {
    private static readonly Range2d<int> NeighborsRange = new(
        Range.FromStartAndEndInclusive(-1, 1),
        Range.FromStartAndEndInclusive(-1, 1)
    );

    public static void Solve(IEnumerable<string> input) {
        var array = input.ToArray();
        var algorithm = array[0];

        var rawMap = Map.From(array.Skip(2));
        var range = Range2d.From(rawMap.ColumnIndices, rawMap.RowIndices);
        var map = rawMap.FindAll('#').ToHashSet();

        EnhanceMany(algorithm, map, range, count: 2).Count.Part1();
        EnhanceMany(algorithm, map, range, count: 50).Count.Part2();
    }

    private static HashSet<V> EnhanceMany(string algorithm, HashSet<V> map, Range2d<int> range, int count) {
        var toggle = algorithm[0] == '#';
        for (var i = 1; i <= count; i++) {
            var borderPixel = toggle ? i % 2 == 0 ? 1 : 0 : 0;
            map = Enhance(algorithm, map, range, borderPixel);
            range = range.Grow(1);
        }

        return map;
    }

    private static HashSet<V> Enhance(string algorithm, IReadOnlySet<V> map, Range2d<int> range, int borderPixel) {
        return range.Grow(1)
            .All()
            .AsParallel()
            .Where(v => algorithm[GetAlgoIndex(v)] == '#')
            .ToHashSet();

        int GetPixel(V arg) {
            if (map.Contains(arg)) return 1;
            return range.Contains(arg) ? 0 : borderPixel;
        }

        int GetAlgoIndex(V pixel) {
            return NeighborsRange.All()
                .Aggregate(0, (res, cur) => res * 2 + GetPixel(pixel + cur));
        }
    }
}