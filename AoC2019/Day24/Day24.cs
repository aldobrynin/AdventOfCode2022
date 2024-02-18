namespace AoC2019.Day24;

public static partial class Day24 {
    public static void Solve(IEnumerable<string> input) {
        var map = Map.From(input);
        var visited = new HashSet<long>();
        Simulate(map).First(rating => !visited.Add(rating)).Part1();
    }

    private static IEnumerable<long> Simulate(Map<char> map) {
        while (true) {
            yield return GetBiodiversityRating(map);
            map = Next(map);
        }
    }

    private static Map<char> Next(Map<char> map) {
        var newMap = map.Clone();
        var adjCount = map.FindAll('#').SelectMany(map.Area4).CountFrequency();
        foreach (var coordinate in map.Coordinates()) {
            newMap[coordinate] = (map[coordinate], adjCount.GetValueOrDefault(coordinate)) switch {
                ('#', not 1) => '.',
                ('.', 1 or 2) => '#',
                _ => newMap[coordinate],
            };
        }

        return newMap;
    }

    private static long GetBiodiversityRating(Map<char> map) {
        return map.FindAll('#')
            .Select(c => c.Y * map.SizeX + c.X)
            .Aggregate(0L, (acc, i) => acc.SetBit((int)i));
    }
}