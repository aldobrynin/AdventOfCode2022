using System.Text.RegularExpressions;
using Common.AoC;
using Range = Common.Range;

namespace AoC2022.Day15;


public record Sensor(V Pos, V ClosestBeacon) {
    private static readonly Regex Regex = new(
        "Sensor at x=(?<x>[\\d\\-]+), y=(?<y>[\\d\\-]+): closest beacon is at x=(?<bx>[\\d\\-]+), y=(?<by>[\\d\\-]+)");

    public int Distance => Pos.DistTo(ClosestBeacon);

    public static Sensor Parse(string input) {
        var match = Regex.Match(input);
        var pos = new V(int.Parse(match.Groups["x"].Value), int.Parse(match.Groups["y"].Value));
        var closestBeacon = new V(int.Parse(match.Groups["bx"].Value), int.Parse(match.Groups["by"].Value));
        return new Sensor(pos, closestBeacon);
    }
}

public partial class Day15 {
    public static void Solve(IEnumerable<string> fileInput) {
        var y = AoCContext.IsSample ? 10 : 2_000_000;

        var sensors = fileInput.Select(Sensor.Parse).ToArray();
        var coveredPositions = GetCoveredPositions(sensors, y).Sum(s => s.Length);
        var beacons = sensors.Select(x => x.ClosestBeacon).Where(x => x.Y == y).Distinct().Count();
        (coveredPositions - beacons).Part1();

        var beacon = GetUnknownBeaconPositions(sensors, Range.FromStartAndEndInclusive(0, y * 2)).Single();
        (beacon.X * 4_000_000L + beacon.Y).Part2();
    }

    private static IEnumerable<Range<int>> GetCoveredPositions(IEnumerable<Sensor> sensors, int y) {
        return sensors
            .Select(x => Range.FromStartAndEndInclusive(x.Pos.X, x.Pos.X).Grow(x.Distance - Math.Abs(x.Pos.Y - y)))
            .Where(x => !x.IsEmpty())
            .Merge();
    }

    private static ParallelQuery<V> GetUnknownBeaconPositions(Sensor[] sensors, Range<int> yRange) {
        return yRange
            .AsParallel()
            .Select(y => (y, ranges: GetCoveredPositions(sensors, y)))
            .SelectMany(y => yRange.Subtract(y.ranges).SelectMany(r => r.Select(x => new V(x, y.y))));
    }
}