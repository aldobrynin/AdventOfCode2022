using System.Text.RegularExpressions;
namespace Solution.Day15;


public record Sensor(V Pos, V ClosestBeacon)
{
    private static readonly Regex Regex = new(
        "Sensor at x=(?<x>[\\d\\-]+), y=(?<y>[\\d\\-]+): closest beacon is at x=(?<bx>[\\d\\-]+), y=(?<by>[\\d\\-]+)");

    public static Sensor Parse(string input)
    {
        var match = Regex.Match(input);
        var pos = new V(int.Parse(match.Groups["x"].Value), int.Parse(match.Groups["y"].Value));
        var closestBeacon = new V(int.Parse(match.Groups["bx"].Value), int.Parse(match.Groups["by"].Value));
        return new Sensor(pos, closestBeacon);
    }
}

public class Day15
{
    public static void Solve(IEnumerable<string> fileInput)
    {
        var y = 2_000_000;
        var sensors = fileInput.Select(Sensor.Parse).ToArray();
        var coveredPositions = GetCoveredPositions(sensors, y).Sum(s => s.Length);
        var beacons = sensors.Select(x => x.ClosestBeacon).Where(x => x.Y == y).Distinct().Count();
        (coveredPositions - beacons).Dump("Part1: ");

        var beacon = GetUnknownBeaconPositions(sensors).First();
        (beacon.X * 4_000_000L + beacon.Y).Dump("Part2: ");
    }

    private static IEnumerable<Range<int>> GetCoveredPositions(Sensor[] sensors1, int i)
    {
        var ranges = new List<Range<int>>();
        foreach (var (pos, closestBeacon) in sensors1)
        {
            var dist = pos.DistTo(closestBeacon);
            if (pos.Y + dist >= i && i >= pos.Y - dist)
            {
                var dx = dist - Math.Abs(pos.Y - i);
                ranges.Add(new Range<int>(pos.X - dx, pos.X + dx));
            }
        }

        return ranges.Merge();
    }

    private static IEnumerable<V> GetUnknownBeaconPositions(Sensor[] sensors)
    {
        var windowRange = new Range<int>(0, 4_000_000);
        for (int y = 0; y <= 4_000_000; y++)
        {
            var rs = GetCoveredPositions(sensors, y).Where(r => r.HasIntersection(windowRange)).ToList();
            for (int x = 0; x < rs[0].From; x++)
                yield return new(x, y);
            for (int i = 0; i < rs.Count - 1; i++)
            for (int x = rs[i].To+1; x < rs[i+1].From; x++)
                yield return new(x, y);
            for (int x = rs.Last().To+1; x <= windowRange.To; x++)
                yield return new(x, y);
        }
    }
}