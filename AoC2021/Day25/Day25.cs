namespace AoC2021.Day25;

public partial class Day25
{
    public static void Solve(IEnumerable<string> input) {
        Simulate(Map.From(input)).Count().Part1();
    }

    private static IEnumerable<(HashSet<V> East, HashSet<V> South, int MovedCount)> Simulate(Map<char> map)
    {
        var yLength = map.SizeY;
        var xLength = map.SizeX;
        var east = map.FindAll('>').ToHashSet();
        var south = map.FindAll('v').ToHashSet();

        int movedCount;
        do yield return (east, south, movedCount) = SimulateRound();
        while (movedCount > 0);

        (HashSet<V>, int) MoveTo(IReadOnlySet<V> movingSet, IReadOnlySet<V> otherSet, V dir)
        {
            var changed = 0;
            var res = new HashSet<V>(movingSet.Count);
            foreach (var (from, to) in movingSet.Select(from => (From: from, To: (from + dir).Mod(yLength, xLength))))
            {
                var next = movingSet.Contains(to) || otherSet.Contains(to) ? from : to;
                res.Add(next);
                if (next == to)
                    changed++;
            }

            return (res, changed);
        }

        (HashSet<V>, HashSet<V>, int) SimulateRound()
        {
            var nextEast = east;
            var nextSouth = south;

            (nextEast, var movedEast) = MoveTo(nextEast, nextSouth, V.Right);
            (nextSouth, var movedSouth) = MoveTo(nextSouth, nextEast, V.Down);

            return (nextEast, nextSouth, movedEast + movedSouth);
        }
    }
}