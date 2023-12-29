using Common.AoC;

namespace AoC2019.Day08;

public static partial class Day08 {
    public static void Solve(IEnumerable<string> input) {
        var width = AoCContext.IsSample ? 2 : 25;
        var height = AoCContext.IsSample ? 2 : 6;
        var layers = input.Single().Chunk(width * height).ToArray();
        layers
            .Select(x => x.CountFrequency())
            .MinBy(x => x.GetValueOrDefault('0'))!
            .Product(x => x.Key is '1' or '2' ? x.Value : 1)
            .Part1();

        var image = Enumerable.Range(0, width * height)
            .Select(i => layers.Select(x => x[i]).First(x => x is '0' or '1'))
            .ToArray();
        image.Select(c => c == '0' ? ' ' : '█')
            .Chunk(width)
            .Select(s => s.StringJoin(string.Empty))
            .Prepend(string.Empty)
            .StringJoin("\n").Part2();
    }
}