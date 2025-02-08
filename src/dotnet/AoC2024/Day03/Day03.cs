using System.Text.RegularExpressions;

namespace AoC2024.Day03;

public static partial class Day03 {
    private static readonly Regex Regex = new Regex(@"mul\((?<x>\d{1,3}),(?<y>\d{1,3})\)|do\(\)|don\'t\(\)", RegexOptions.Compiled);

    public static void Solve(IEnumerable<string> input) {
        var lines = input.ToArray();
        lines
            .SelectMany(line => Regex.Matches(line))
            .Where(match => match.Value.StartsWith("mul"))
            .Sum(match => match.Groups["x"].Value.ToInt() * match.Groups["y"].Value.ToInt())
            .Part1();


        lines.SelectMany(line => Regex.Matches(line))
            .Aggregate((Sum: 0, Enabled: true), (accumulator, current) =>
                (accumulator.Enabled, current.Value) switch {
                    (_, "do()") => (accumulator.Sum, true),
                    (_, "don't()") => (accumulator.Sum, false),
                    (false, _) => accumulator,
                    _ => (accumulator.Sum + current.Groups["x"].Value.ToInt() * current.Groups["y"].Value.ToInt(),
                        accumulator.Enabled),
                }
            )
            .Sum
            .Part2();
    }
}
