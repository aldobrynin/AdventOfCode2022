namespace AoC2019.Day03;

public static partial class Day03 {
    public static void Solve(IEnumerable<string> input) {
        var lines = input.Select(s => s.Split(',')).ToArray();
        var first = FindVisitedPoints(lines[0]);
        var second = FindVisitedPoints(lines[1]);

        first.Intersect(second).Min(x => x.MLen).Part1();

        first.WithIndex()
            .Join(second.WithIndex(), x => x.Element, x => x.Element,
                (x, y) => (Step1: x.Index + 1, Step2: y.Index + 1))
            .Min(x => x.Step1 + x.Step2)
            .Part2();
    }

    private static IReadOnlyCollection<V> FindVisitedPoints(string[] line) {
        var current = V.Zero;
        var list = new List<V>();
        foreach (var s in line) {
            var dir = s[0] switch {
                'U' => V.Down,
                'D' => V.Up,
                'L' => V.Left,
                'R' => V.Right,
                _ => throw new ArgumentOutOfRangeException()
            };
            var distance = s[1..].ToInt();
            list.AddRange(1.RangeTo(distance).Select(v => current + v * dir));
            current = list[^1];
        }

        return list;
    }
}