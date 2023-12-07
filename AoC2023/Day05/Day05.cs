namespace AoC2023.Day05;

public class Day5 {
    private record MapRange(RangeLong Src, RangeLong Dst) {
        public long Offset => Dst.From - Src.From;

        public static MapRange Create(string source) {
            var values = source.ToLongArray();
            return new MapRange(RangeLong.FromStartAndLength(values[1], values[2]),
                RangeLong.FromStartAndLength(values[0], values[2]));
        }

        public override string ToString() => $"{Src} => {Dst}";
    }

    public static void Solve(IEnumerable<string> input) {
        var blocks = input.SplitBy(string.IsNullOrEmpty).ToArray();
        var seeds = blocks.First().Single().Split(':')[1].ToLongArray();
        var seedsRanges = seeds.Chunk(2)
            .Select(x => RangeLong.FromStartAndLength(start: x[0], length: x[1]))
            .ToArray();
        var maps = blocks.Skip(1)
            .Select(map => map.Skip(1).Select(MapRange.Create).ToArray())
            .ToArray();

        TranslateValues(seeds).Min().Dump("Part1: ");
        TranslateRanges(seedsRanges).Min(c => c.From).Dump("Part2: ");

        long[] TranslateValues(long[] origin) =>
            maps.Aggregate(origin,
                (values, map) =>
                    values.Select(
                            cur => cur + map.Where(m => m.Src.Contains(cur))
                                .Select(x => x.Offset)
                                .FirstOrDefault(0))
                        .ToArray()
            );

        IEnumerable<RangeLong> TranslateRanges(RangeLong[] ranges) =>
            maps.Aggregate(ranges,
                (current, map) => current.SelectMany(range =>
                        map.Where(m => m.Src.HasIntersection(range))
                            .Select(m => m.Src.IntersectOrThrow(range) + m.Offset)
                            .Concat(range.Subtract(map.Select(s => s.Src))))
                    .ToArray()
            );
    }
}