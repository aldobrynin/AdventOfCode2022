namespace AoC2024.Day05;

public static partial class Day05 {
    public static void Solve(IEnumerable<string> input) {
        var sections = input.SplitBy(string.IsNullOrWhiteSpace).ToArray();

        var rules = sections[0]
            .Select(s => s.ToIntArray("|"))
            .Select(x => (Before: x[0], After: x[1]))
            .ToArray();

        var updates = sections[1]
            .Select(s => s.ToIntArray())
            .ToArray();

        updates
            .Where(IsValid)
            .Sum(u => u.ElementAt(u.Length / 2))
            .Part1();

        var comparer = Comparer<int>.Create((a, b) => rules.Contains((a, b)) ? 1 : -1);
        updates
            .Where(x => !IsValid(x))
            .Pipe(u => Array.Sort(u, comparer))
            .Sum(u => u.ElementAt(u.Length / 2))
            .Part2();

        bool IsValid(int[] update) {
            return rules
                .Select(x => (Before: Array.IndexOf(update, x.Before), After: Array.IndexOf(update, x.After)))
                .All(x => x.Before == -1 || x.After == -1 || x.Before < x.After);
        }
    }
}
