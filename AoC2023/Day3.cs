namespace AoC2023;

public class Day3 {
    public static void Solve(IEnumerable<string> input) {
        var map = Map.From(input);
        var numbers = ReadNumbers().ToArray();

        numbers.Where(x => Area8(x.Coordinates).Any(IsSymbol))
            .Sum(x => x.Number)
            .Dump("Part1: ");

        numbers.SelectMany(x => Area8(x.Coordinates).Where(IsGear).Select(n => (x.Number, Gear: n)))
            .GroupBy(x => x.Gear, x => x.Number)
            .Where(x => x.Count() == 2)
            .Sum(x => x.Product())
            .Dump("Part2: ");

        IEnumerable<(int Number, IReadOnlyCollection<V> Coordinates)> ReadNumbers() {
            var number = 0;
            var cells = new List<V>();
            foreach (var v in map.Coordinates()) {
                if (char.IsDigit(map[v])) {
                    number = number * 10 + (map[v] - '0');
                    cells.Add(v);
                }

                if (char.IsDigit(map[v]) && v.X != map.SizeX - 1) continue;
                yield return (number, cells.ToArray());
                number = 0;
                cells.Clear();
            }
        }

        bool IsSymbol(V v) => map[v] != '.' && !char.IsDigit(map[v]);
        bool IsGear(V v) => map[v] == '*';

        IEnumerable<V> Area8(IEnumerable<V> coordinates) => coordinates.SelectMany(map.Area8).Distinct();
    }
}