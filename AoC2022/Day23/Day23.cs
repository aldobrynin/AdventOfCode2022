namespace AoC2022.Day23;

public partial class Day23
{
    public static void Solve(IEnumerable<string> input)
    {
        var map = input
            .Select(x => x.ToCharArray())
            .ToArray();
        var elvesSet = map.Coordinates().Where(v => map.Get(v) == '#').ToHashSet();

        SimulateElvesPositions(elvesSet)
            .Skip(9)
            .Select(GetEmptyPointsCount)
            .First()
            .Part1();

        SimulateElvesPositions(elvesSet)
            .Count()
            .Part2();
    }

    private static int GetEmptyPointsCount(IReadOnlyCollection<V> points)
    {
        var (min, max) = GetBorders(points);
        var totalPoints = (max.X - min.X + 1) * (max.Y - min.Y + 1);
        return totalPoints - points.Count;
    }

    private static IEnumerable<IReadOnlyCollection<V>> SimulateElvesPositions(IReadOnlySet<V> elvesSet)
    {
        var round = 0;
        (V From, V To, bool StayPut) ToProposition(V elf)
        {
            bool HasElf(V v) => elvesSet.Contains(v);
            var proposedMove = HasAnyNeighbor(HasElf, elf) ? ProposeMove(HasElf, elf, round) : elf;
            return (From: elf, To: proposedMove, elf == proposedMove);
        }

        var isFinalState = false;
        while (!isFinalState)
        {
            var proposedPositions = elvesSet.Select(ToProposition).ToArray();
            elvesSet = proposedPositions
                .GroupBy(x => x.To)
                .SelectMany(group => group.Select(c => group.Skip(1).Any() ? c.From : c.To))
                .ToHashSet();
            isFinalState = proposedPositions.All(x => x.StayPut);
            yield return elvesSet;
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

    private static readonly V[][] Dirs =
    {
        new[] { V.N, V.NE, V.NW },
        new[] { V.S, V.SE, V.SW },
        new[] { V.W, V.NW, V.SW },
        new[] { V.E, V.NE, V.SE },
    };

    private static bool HasAnyNeighbor(Func<V, bool>  hasElf, V elf) =>
        V.Directions8.Any(dir => hasElf(elf + dir));

    private static V ProposeMove(Func<V, bool> hasElf, V elf, int round)
    {
        var nextDirection = Enumerable.Range(0, Dirs.Length)
            .Select(i => Dirs[(i + round) % Dirs.Length])
            .FirstOrDefault(dir => dir.All(d => !hasElf(d + elf)))
            ?[0] ?? V.Zero;
        return elf + nextDirection;
    }
}