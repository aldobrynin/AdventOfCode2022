namespace AoC2023.Day11;

public partial class Day11 {
    public static void Solve(IEnumerable<string> input) {
        var map = Map.From(input);

        var emptyRowsCount = map.Rows().RunningSum(row => row.All(c => c == '.') ? 1 : 0).Prepend(0).ToArray();
        var emptyColsCount = map.Columns().RunningSum(col => col.All(c => c == '.') ? 1 : 0).Prepend(0).ToArray();

        var galaxies = map.FindAll('#').ToArray();
        GetDistances(galaxies, factor: 2)
            .Sum()
            .Part1();

        GetDistances(galaxies, factor: AoCContext.IsSample ? 10 : 1000000)
            .Sum()
            .Part2();

        IEnumerable<long> GetDistances(IEnumerable<V> points, int factor) =>
            points
                .Select(v => v + (factor - 1) * new V(emptyColsCount[v.X], emptyRowsCount[v.Y]))
                .Pairs()
                .Select(x => (long)x.A.DistTo(x.B));
    }
}