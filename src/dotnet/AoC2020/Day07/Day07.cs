namespace AoC2020.Day07;

public partial class Day07
{
    private record Rule(int Count, string Color)
    {
        public static Rule Parse(string s)
        {
            var split = s.Split(' ', 2);
            return new Rule(int.Parse(split[0]), split[1]);
        }
    }

    public static void Solve(IEnumerable<string> input)
    {
        var parsed = input.Select(line =>
                line.Replace(" bags", string.Empty)
                    .Replace(" bag", string.Empty)
                    .Replace(".", string.Empty)
                    .Replace("no other", string.Empty)
                    .Split(" contain "))
            .ToDictionary(x => x[0],
                x => x[1].Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .Select(s => s.Trim())
                    .Select(Rule.Parse)
                    .ToArray());

        var target = new[] { "shiny gold" };
        var colors = new HashSet<string>();

        while (target.Length > 0)
        {
            target = parsed
                .Where(x => !colors.Contains(x.Key))
                .Where(x => x.Value.Any(v => target.Contains(v.Color)))
                .Select(x => x.Key)
                .ToArray();
            colors.UnionWith(target);
        }

        colors.Count.Part1();

        Get("shiny gold", parsed).Part2();
    }

    private static long Get(string cur, IReadOnlyDictionary<string, Rule[]> rules)
    {
        return rules[cur].Sum(rule => rule.Count + rule.Count * Get(rule.Color, rules));
    }
}