using System.Collections.Immutable;
using Range = Common.Range;
using Variables = System.Collections.Immutable.ImmutableDictionary<string, Common.Range<long>>;

namespace AoC2023.Day19;

public static partial class Day19 {
    public static void Solve(IEnumerable<string> input) {
        var blocks = input.SplitBy(string.IsNullOrEmpty).ToArray();
        var workflows = blocks[0].Select(Workflow.Parse).ToDictionary(x => x.Name);
        var parts = blocks[1].Select(line => line
                .Split(new[] { ',', '=', '{', '}' }, StringSplitOptions.RemoveEmptyEntries)
                .Chunk(2).ToDictionary(x => x[0], x => x[1].ToInt()))
            .ToArray();

        parts.Where(p => IsAccepted(p)).Sum(p => p.Values.Sum()).Part1();

        var range = Range.FromStartAndEndInclusive(1L, 4000);
        var initialVariables = "xmas".ToImmutableDictionary(x => x.ToString(), _ => range);

        CountAccepted("in", initialVariables).Part2();

        long CountAccepted(string current, Variables variables) => current switch {
            "R" => 0L,
            "A" => variables.Values.Product(x => x.Length),
            _ when variables.Values.Any(r => r.IsEmpty()) => 0L,
            _ => workflows[current].Branches.Aggregate((Count: 0L, Current: (Variables?)variables), (acc, next) => {
                if (acc.Current == null) return acc;
                var nextBranch = Match(acc.Current, next.Condition);
                return (acc.Count + CountAccepted(next.NextWorkflow, nextBranch.If), nextBranch.Else);
            }).Count
        };

        bool IsAccepted(Dictionary<string, int> part, string current = "in") => current switch {
            "A" => true,
            "R" => false,
            _ => IsAccepted(part, workflows[current].GetNext(part))
        };
    }

    private static (Variables If, Variables? Else) Match(Variables variables, Condition? variableRange) {
        if (variableRange == null) return (variables, null);
        var current = variables[variableRange.RatingName];
        var @if = current.Intersect(variableRange.Range) ?? Range<long>.Empty;
        var @else = current.Subtract(variableRange.Range);
        var elseRange = @else.Left ?? @else.Right ?? Range<long>.Empty;

        return (variables.SetItem(variableRange.RatingName, @if),
            variables.SetItem(variableRange.RatingName, elseRange));
    }

    private record Condition(string RatingName, Range<long> Range);

    private record Workflow(string Name, (Condition? Condition, string NextWorkflow)[] Branches) {
        public static Workflow Parse(string line) {
            var nameAndConditions = line.Split('{');
            var conditions = nameAndConditions[1].TrimEnd('}').Split(',')
                .Select(x => x.Split(':'))
                .Select(s => (s.Length == 1 ? null : ParseRange(s[0]), s.Last()))
                .ToArray();
            return new Workflow(nameAndConditions[0], conditions);

            Condition ParseRange(string s) {
                var value = s[2..].ToLong();
                var range = s[1] switch {
                    '>' => Range.FromStartAndEndInclusive(value + 1, 4000),
                    '<' => Range.FromStartAndEnd(1, value),
                    _ => throw new ArgumentOutOfRangeException(),
                };
                return new Condition(s[0].ToString(), range);
            }
        }

        public string GetNext(Dictionary<string, int> part) => Branches
            .First(b => b.Condition is null || b.Condition.Range.Contains(part[b.Condition.RatingName]))
            .NextWorkflow;
    }
}