namespace AoC2021.Day25;

public class Day25
{
    public static void Solve(IEnumerable<string> input)
    {
        var array = input.Select(line => line.ToCharArray()).ToArray();

        Simulate(array).Count().Dump("Part1: ");
    }

    private static IEnumerable<(HashSet<V> East, HashSet<V> South, int MovedCount)> Simulate(char[][] map)
    {
        var yLength = map.Length;
        var xLength = map[0].Length;
        var east = map.Coordinates()
            .Where(v => map.Get(v) == '>')
            .ToHashSet();
        var south = map.Coordinates()
            .Where(v => map.Get(v) == 'v')
            .ToHashSet();

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
            (nextSouth, var movedSouth) = MoveTo(nextSouth, nextEast, V.Up);

            return (nextEast, nextSouth, movedEast + movedSouth);
        }
    }
}