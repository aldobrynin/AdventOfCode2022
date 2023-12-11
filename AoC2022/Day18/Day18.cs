using Range = Common.Range;

namespace AoC2022.Day18;

public partial class Day18 {
    public static void Solve(IEnumerable<string> fileInput) {
        var lavaCubes = fileInput
            .Select(V3.Parse)
            .ToArray();
        var lavaSet = lavaCubes.ToHashSet();

        lavaCubes
            .Sum(cube => cube.Neighbors6().Count(d => !lavaSet.Contains(d)))
            .Part1();

        var maxOuterV = new V3(lavaCubes.Max(v => v.X) + 1, lavaCubes.Max(v => v.Y) + 1, lavaCubes.Max(v => v.Z) + 1);
        var minOuterV = new V3(lavaCubes.Min(v => v.X) - 1, lavaCubes.Min(v => v.Y) - 1, lavaCubes.Min(v => v.Z) - 1);
        var maxDistance = maxOuterV.DistTo(minOuterV);
        var outerRange = new Range3d(
            Range.FromStartAndEndInclusive(minOuterV.X, maxOuterV.X),
            Range.FromStartAndEndInclusive(minOuterV.Y, maxOuterV.Y),
            Range.FromStartAndEndInclusive(minOuterV.Z, maxOuterV.Z)
        );

        var outerWaterSet = SearchHelpers
            .Bfs(from => from.Neighbors6().Where(v => !lavaSet.Contains(v) && outerRange.Contains(v)),
                maxDistance, initialStates: maxOuterV)
            .Select(x => x.State)
            .ToHashSet();

        lavaCubes
            .Sum(cube => cube.Neighbors6().Count(d => outerWaterSet.Contains(d)))
            .Part2();
    }
}