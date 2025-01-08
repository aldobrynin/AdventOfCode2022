using Common.AoC;
using Range = Common.Range;

namespace AoC2018.Day06;

public static partial class Day06 {
    public static void Solve(IEnumerable<string> input) {
        var points = input.Select(V.Parse).ToArray();
        var maxDistance = AoCContext.IsSample ? 32 : 10_000;

        var xRange = Range.FromStartAndEndInclusive(points.Min(x => x.X), points.Max(x => x.X));
        var yRange = Range.FromStartAndEndInclusive(points.Min(x => x.Y), points.Max(x => x.Y));
        var range = Range2d.From(xRange, yRange);

        var allPoints = Range2d.From(xRange, yRange)
            .Grow(1)
            .All()
            .Select(v => (V: v, Distances: points.Select(p => (p, Distance: v.DistTo(p))).ToArray()))
            .ToArray();

        allPoints
            .Select(v => (
                v.V,
                Closest: v.Distances
                    .GroupBy(x => x.Distance, x => x.p)
                    .MinBy(x => x.Key)?
                    .ToArray() ?? []
            ))
            .Where(x => x.Closest.Length == 1)
            .GroupBy(x => x.Closest.Single(), x => x.V)
            .Where(s => s.All(range.Contains))
            .Max(x => x.Count())
            .Part1();

        allPoints
            .Count(x => x.Distances.Sum(d => d.Distance) < maxDistance)
            .Part2();
    }
}
