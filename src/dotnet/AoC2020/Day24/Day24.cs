namespace AoC2020.Day24;

public partial class Day24 {
    private static readonly Dictionary<string, V> Map = new() {
        ["sw"] = new V(-1, -3),
        ["se"] = new V(+1, -3),
        ["nw"] = new V(-1, +3),
        ["ne"] = new V(+1, +3),
        ["e"] = new V(+2, 0),
        ["w"] = new V(-2, 0),
    };

    public static void Solve(IEnumerable<string> input) {
        var blackTiles = input.Select(Parse)
            .Select(tile => tile.Aggregate(V.Zero, (v, dir) => v + Map[dir]))
            .CountFrequency()
            .Where(x => x.Value % 2 == 1)
            .Select(x => x.Key)
            .ToHashSet();

        blackTiles.Count.Part1();

        Simulate(blackTiles, limit: 100)
            .Last()
            .Part2();
    }

    private static IEnumerable<int> Simulate(HashSet<V> blackTiles, int limit) {
        var day = 0;
        while (day++ < limit) {
            blackTiles = blackTiles.SelectMany(HexagonalNeighbors)
                .CountFrequency()
                .Where(x => x.Value == 2 || blackTiles.Contains(x.Key) && x.Value == 1)
                .Select(x => x.Key)
                .ToHashSet();
            yield return blackTiles.Count;
        }
    }

    private static IEnumerable<V> HexagonalNeighbors(V point) => Map.Values.Select(v => point + v);

    private static IList<string> Parse(string line) {
        var res = new List<string>();
        for (var i = 0; i < line.Length;) {
            var length = line[i] is 's' or 'n' ? 2 : 1;
            res.Add(line[i..(i + length)]);
            i += length;
        }

        return res;
    }
}