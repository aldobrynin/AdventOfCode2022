using Common;

namespace Solution;

public class Day23
{
    public static void Solve(IEnumerable<string> input)
    {
        var map = input
            .Select(x => x.ToCharArray())
            .ToArray();
        var elvesSet = map.Coordinates().Where(v => map.Get(v) == '#').ToHashSet();

        SimulateElvesPositions(elvesSet)
            .Take(10)
            .Select(GetEmptyPointsCount)
            .Last()
            .Dump("Part1: ");

        SimulateElvesPositions(elvesSet)
            .Count()
            .Dump("Part2: ");
    }

    private static int GetEmptyPointsCount(IReadOnlyCollection<V> points)
    {
        var (min, max) = GetBorders(points);
        var totalPoints = (max.X - min.X + 1) * (max.Y - min.Y + 1);
        return totalPoints - points.Count;
    }

    private static IEnumerable<IReadOnlyCollection<V>> SimulateElvesPositions(IReadOnlySet<V> initialPositions)
    {
        var round = 0;
        var elvesSet = initialPositions;

        while (round < 1000)
        {
            var proposedPositions = new List<(V From, V To)>(elvesSet.Count);
            var isFinalState = true;
            foreach (var elf in elvesSet)
            {
                var nextPosition = HasAnyNeighbor(elvesSet, elf) ? ProposeMove(elvesSet, elf, round) : elf;
                if (elf != nextPosition)
                    isFinalState = false;
                proposedPositions.Add((elf, nextPosition));
            }

            elvesSet = proposedPositions
                .GroupBy(x => x.To, (_, s) => s.ToArray())
                .SelectMany(x => x.Length == 1 ? new[] { x[0].To } : x.Select(c => c.From))
                .ToHashSet();

            yield return elvesSet;
            if (isFinalState)
                yield break;
            round++;
        }
    }

    private static (V Min, V Max) GetBorders(IReadOnlyCollection<V> points)
    {
        var minX = points.Min(x => x.X);
        var maxX = points.Max(x => x.X);
        var minY = points.Min(x => x.Y);
        var maxY = points.Max(x => x.Y);
        return (new(minX, minY), new(maxX, maxY));
    }

    private static IEnumerable<V[]> GetMoves(int round)
    {
        var dirs = new[]
        {
            new[] { V.S, V.SE, V.SW },
            new[] { V.N, V.NE, V.NW },
            new[] { V.W, V.NW, V.SW },
            new[] { V.E, V.NE, V.SE },
        };

        for (var i = 0; i < dirs.Length; i++)
            yield return dirs[(i + round) % dirs.Length];
    }

    private static bool HasAnyNeighbor(IReadOnlySet<V> elvesSet, V elf) =>
        V.Directions8.Any(dir => elvesSet.Contains(elf + dir));

    private static V ProposeMove(IReadOnlySet<V> elvesSet, V elf, int round)
    {
        var nextDirection = GetMoves(round).FirstOrDefault(dir => dir.All(d => !elvesSet.Contains(d + elf)));
        return elf + (nextDirection != null ? nextDirection[0] : V.Zero);
    }
}