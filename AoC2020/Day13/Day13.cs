namespace AoC2020.Day13;

public class Day13 {
    public static void Solve(IEnumerable<string> input) {
        var inputArray = input.ToArray();
        var arrivalTimestamp = int.Parse(inputArray[0]);

        var buses = inputArray[1]
            .Split(',')
            .Select((x, index) => (Index: index, Id: x == "x" ? -1 : long.Parse(x)))
            .Where(x => x.Id != -1)
            .ToArray();

        var closestBus = buses
            .Select(bus => (bus.Id, MinutesToWait: bus.Id - arrivalTimestamp % bus.Id))
            .MinBy(x => x.MinutesToWait);

        (closestBus.Id * closestBus.MinutesToWait).Dump("Part1: ");
        Part2(buses).Dump("Part2: ");
    }

    private static long Part2(IReadOnlyList<(int Index, long Id)> buses) {
        var timestamp = 0L;
        var step = buses[0].Id;

        foreach (var (index, busId) in buses.Skip(1)) {
            while ((timestamp + index) % busId != 0) timestamp += step;
            step *= busId;
        }

        return timestamp;
    }
}