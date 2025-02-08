namespace AoC2018.Day02;

public static partial class Day02 {
    public static void Solve(IEnumerable<string> input) {
        var boxes = input.ToArray();
        boxes
            .Select(x => x.CountFrequency())
            .Select(x => (Two: x.Any(kv => kv.Value == 2), Three: x.Any(kv => kv.Value == 3)))
            .Aggregate((0, 0), (acc, cur) => (acc.Item1 + (cur.Two ? 1 : 0), acc.Item2 + (cur.Three ? 1 : 0)))
            .Apply(x => x.Item1 * x.Item2)
            .Part1();

        boxes.Pairs()
            .Single(x => x.A.Zip(x.B).Count(y => y.First != y.Second) == 1)
            .Apply(x => x.A.Zip(x.B).Where(y => y.First == y.Second).Select(y => y.First).StringJoin(""))
            .Part2();
    }
}
