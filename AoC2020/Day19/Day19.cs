using System.Collections.Immutable;
using System.Text.RegularExpressions;

namespace AoC2020.Day19;

public partial class Day19 {
    public static void Solve(IEnumerable<string> input) {
        var rawRules = input.TakeWhile(x => !string.IsNullOrWhiteSpace(x))
            .Select(x => x.Split(": "))
            .ToImmutableDictionary(x => x[0], x => x[1].Trim('"'));

        var messages = input.Skip(rawRules.Count + 1).ToArray();

        var compiledRule = new Regex($"^{CompileRule("0", rawRules)}$", RegexOptions.Compiled);
        messages
            .Count(message => compiledRule.IsMatch(message))
            .Part1();


        var part2Rules = rawRules.SetItem("8", "42 | 42 8")
            .SetItem("11", "42 31 | 42 11 31");
        var compiledRule2 = new Regex($"^{CompileRule("0", part2Rules)}$", RegexOptions.Compiled);
        messages
            .Count(message => compiledRule2.IsMatch(message))
            .Part2();
    }

    private static string CompileRule(string ruleKey, IReadOnlyDictionary<string, string> rawRules) {
        var rules = new Dictionary<string, string>();
        return Parse(ruleKey);

        string Parse(string key) {
            if (rules.TryGetValue(key, out var rule))
                return rule;
            return rules[key] = ParseIn(key);
        }

        string ParseIn(string key) {
            var source = rawRules[key];
            if (char.IsLetter(source, 0))
                return source;

            if (key == "8" && source == "42 | 42 8")
                return $"{Parse("42")}+";

            if (key == "11" && source == "42 31 | 42 11 31")
                return $"(?<left>{Parse("42")})+(?<-left>{Parse("31")})+";

            var parsedRules = source.Split(" | ")
                .Select(part => part.Split(' ').Select(Parse).StringJoin(string.Empty))
                .ToArray();
            return parsedRules.Length > 1 ? $"({parsedRules.StringJoin("|")})" : parsedRules.Single();
        }
    }
}