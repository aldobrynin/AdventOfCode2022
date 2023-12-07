namespace AoC2023.Day06;

public class Day6 {
    public static void Solve(IEnumerable<string> input) {
        var values = input.Select(line => line.Split(':')[1]).ToArray();
        var times = values[0];
        var distances = values[1];
        times.ToLongArray()
            .Zip(distances.ToLongArray(), Solve)
            .Product()
            .Dump("Part1: ");

        var totalTime = times.Replace(" ", string.Empty).ToLong();
        var totalDistance = distances.Replace(" ", string.Empty).ToLong();
        Solve(totalTime, totalDistance)
            .Dump("Part2: ");
    }

    // raceTime*x - x^2 > distance
    // x^2 - raceTime*x + distance < 0
    private static long Solve(long raceTime, long distance) {
        var discriminant = Math.Sqrt(raceTime * raceTime - 4 * distance);
        var x1 = (raceTime + discriminant) / 2;
        var x2 = (raceTime - discriminant) / 2;
        return (long)(Math.Ceiling(x1) - 1 - Math.Floor(x2));
    }
}