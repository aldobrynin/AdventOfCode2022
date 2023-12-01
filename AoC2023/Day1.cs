namespace AoC2023;

public class Day1 {
    public static void Solve(IEnumerable<string> input) {
        input.Select(line => int.Parse($"{line.First(char.IsDigit)}{line.Last(char.IsDigit)}"))
            .Sum()
            .Dump("Part1: ");

        input
            .Select(line => Parse(line).ToArray())
            .Select(x => x.First() * 10L + x.Last())
            .Sum()
            .Dump("Part2: ");
    }

    private static IEnumerable<int> Parse(string s) {
        var map = new Dictionary<string, int> {
            ["one"] = 1,
            ["two"] = 2,
            ["three"] = 3,
            ["four"] = 4,
            ["five"] = 5,
            ["six"] = 6,
            ["seven"] = 7,
            ["eight"] = 8,
            ["nine"] = 9,
        };
        for (var i = 0; i < s.Length; i++) {
            if (char.IsDigit(s[i])) yield return s[i] - '0';
            else {
                var index = i;
                var match = map.FirstOrDefault(kv => s.IndexOf(kv.Key, index, StringComparison.Ordinal) == index);
                if (match.Key != null) yield return match.Value;
            }
        }
    }
}