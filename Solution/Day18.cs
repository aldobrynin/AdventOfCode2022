namespace Solution;
using Common;

public class Day18
{
    public record State(V3 Current, int Distance);

    public static void Solve(IEnumerable<string> fileInput)
    {
        var lavaCubes = fileInput
            .Select(V3.Parse)
            .ToArray();
        var lavaSet = lavaCubes.ToHashSet();

        lavaCubes
            .Sum(cube => cube.Neighbors6().Count(d => !lavaSet.Contains(d)))
            .Dump("Part1: ");

        var maxOuterV = new V3(lavaCubes.Max(v => v.X) + 1, lavaCubes.Max(v => v.Y) + 1, lavaCubes.Max(v => v.Z) + 1);
        var minOuterV = new V3(lavaCubes.Min(v => v.X) - 1, lavaCubes.Min(v => v.Y) - 1, lavaCubes.Min(v => v.Z) - 1);
        var maxDistance = maxOuterV.DistTo(minOuterV);

        var queue = new Queue<State>();
        queue.Enqueue(new State(maxOuterV, 0));
        var outerWaterSet = new HashSet<V3> { maxOuterV };

        while (queue.TryDequeue(out var item))
        {
            foreach (var candidate in item.Current.Neighbors6()
                         .Where(v => !lavaSet.Contains(v))
                         .Where(v => v.X <= maxOuterV.X && v.Y <= maxOuterV.Y && v.Z <= maxOuterV.Z)
                         .Where(outerWaterSet.Add)
                         .Select(v => new State(v, item.Distance + 1))
                         .Where(s => s.Distance < maxDistance)
                    )
                queue.Enqueue(candidate);
        }

        lavaCubes
            .Sum(cube => cube.Neighbors6().Count(d => outerWaterSet.Contains(d)))
            .Dump("Part2: ");
    }
}