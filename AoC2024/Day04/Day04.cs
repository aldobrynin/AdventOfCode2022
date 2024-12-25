namespace AoC2024.Day04;

public static partial class Day04 {
    public static void Solve(IEnumerable<string> input) {
        var map = Map.From(input);

        map.Coordinates()
            .Sum(v => V.Directions8.Count(dir => ReadWord(v, dir, 4) is "XMAS"))
            .Part1();

        map.Coordinates()
            .Count(v => new[] { V.SE, V.NE }.All(dir => ReadWord(v - dir, dir, 3) is "MAS" or "SAM"))
            .Part2();


        string ReadWord(V start, V direction, int length) {
            return Line(start, direction)
                .Take(length)
                .Select(x => map.GetValueOrDefault(x, default))
                .StringJoin(string.Empty);
        }
    }

    private static IEnumerable<V> Line(V from, V dir) {
        while (true) {
            yield return from;
            from += dir;
        }
    }
}
