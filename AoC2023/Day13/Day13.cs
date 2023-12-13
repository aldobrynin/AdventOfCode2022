namespace AoC2023.Day13;

public partial class Day13 {
    public static void Solve(IEnumerable<string> input) {
        var maps = input.SplitBy(string.IsNullOrWhiteSpace).Select(Map.From).ToArray();

        maps.Sum(map => GetScore(map, FindReflectionLines)).Part1();
        maps.Sum(map => GetScore(map, FindReflectionLinesAfterFix)).Part2();
    }

    private static long GetScore(Map<char> map, Func<Map<char>, IEnumerable<int>> getLinesFunc) =>
        getLinesFunc(map.Transpose()).SingleOrDefault(int.IsPositive)
        + 100L * getLinesFunc(map).SingleOrDefault(int.IsPositive);

    private static IEnumerable<int> FindReflectionLinesAfterFix(Map<char> map) {
        var beforeFix = FindReflectionLines(map);
        var lines = new HashSet<int>();
        foreach (var coordinate in map.Coordinates()) {
            Flip(coordinate);
            lines.UnionWith(FindReflectionLines(map));
            Flip(coordinate);
        }

        return lines.Except(beforeFix);

        void Flip(V coordinate) => map[coordinate] = map[coordinate] == '#' ? '.' : '#';
    }

    private static IEnumerable<int> FindReflectionLines(Map<char> map) =>
        map.RowIndices.Skip(1).Where(rowIndex => IsReflectionLine(map, rowIndex));

    private static bool IsReflectionLine(Map<char> map, int rowInd) {
        var mirrorSize = Math.Min(rowInd, map.SizeY - rowInd);
        return 0.RangeTo(mirrorSize)
            .All(offset => AreRowsEqual(map, rowInd - offset - 1, rowInd + offset));
    }

    private static bool AreRowsEqual(Map<char> map, int rowInd1, int rowInd2) =>
        map.ColumnIndices.All(col => map[rowInd1, col] == map[rowInd2, col]);
}