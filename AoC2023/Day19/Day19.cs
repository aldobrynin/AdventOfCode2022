using System.Collections.Immutable;
using Range = Common.Range;
using Variables = System.Collections.Immutable.ImmutableDictionary<string, Common.Range<long>>;

namespace AoC2023.Day19;

public static partial class Day19 {
    private record Condition(string VariableName, Range<long> Range) {
        public bool Satisfies(Dictionary<string, int> part) => Range.Contains(part[VariableName]);
    }

    private record Workflow(
        string Name,
        IReadOnlyCollection<(Condition? VariableRange, string NextWorkflow)> Branches) {
        public static Workflow Parse(string line) {
            var nameAndConditions = line.Split('{');
            var conditionsRaw = nameAndConditions[1].TrimEnd('}').Split(',');
            var conditions = conditionsRaw
                .Select(c => {
                    var parts = c.Split(':');
                    return (condition: parts.Length == 1 ? null : ParseRange(parts[0]), next: parts.Last());
                }).ToArray();
            return new Workflow(nameAndConditions[0], conditions);

            Condition? ParseRange(string s) {
                var value = s[2..].ToLong();
                var range = s[1] switch {
                    '>' => Range.FromStartAndEndInclusive(value + 1, 4000),
                    '<' => Range.FromStartAndEnd(1, value),
                    _ => throw new ArgumentOutOfRangeException(),
                };
                var condition = (Condition?)new Condition(s[0].ToString(), range);
                return condition;
            }
        }

        public string GetNext(Dictionary<string, int> part) {
            return Branches
                .First(b => b.VariableRange is null || b.VariableRange.Satisfies(part))
                .NextWorkflow;
        }
    }

    public static void Solve(IEnumerable<string> input) {
        var blocks = input.SplitBy(string.IsNullOrEmpty).ToArray();
        var workflows = blocks[0].Select(Workflow.Parse).ToDictionary(x => x.Name);
        var parts = blocks[1]
            .Select(line => line.Trim('{', '}').Split(',', '=').Chunk(2)
                .ToDictionary(x => x[0], x => x[1].ToInt()))
            .ToArray();

        parts.Where(IsAccepted).Sum(p => p.Values.Sum()).Part1();

        var initialVariables = "xmas"
            .ToImmutableDictionary(x => x.ToString(), _ => Range.FromStartAndEndInclusive(1L, 4000));

        CountAccepted("in", initialVariables).Part2();

        long CountAccepted(string current, Variables variables) {
            if (current == "R" || variables.Values.Any(r => r.IsEmpty())) return 0L;
            if (current == "A") return variables.Values.Product(x => x.Length);

            var count = 0L;
            foreach (var (variableRange, nextWorkflow) in workflows[current].Branches) {
                var nextBranch = Match(variables, variableRange);
                count += CountAccepted(nextWorkflow, nextBranch.If);
                if (nextBranch.Else is null) break;
                variables = nextBranch.Else;
            }

            return count;
        }

        bool IsAccepted(Dictionary<string, int> part) {
            var current = "in";
            while (current != "A" && current != "R") current = workflows[current].GetNext(part);
            return current == "A";
        }
    }

    private static (Variables If, Variables? Else) Match(Variables variables, Condition? variableRange) {
        if (variableRange == null) return (variables, null);
        var current = variables[variableRange.VariableName];
        var @if = current.Intersect(variableRange.Range) ?? Range<long>.Empty;
        var @else = current.Subtract(variableRange.Range);
        var elseRange = @else.Left ?? @else.Right ?? Range<long>.Empty;

        return (variables.SetItem(variableRange.VariableName, @if),
            variables.SetItem(variableRange.VariableName, elseRange));
    }
}