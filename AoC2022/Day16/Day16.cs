using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

namespace Solution.Day16;

public record State(long OpenMask, int ValveIndex);
public record ValveData(int Index, string Name, int Rate, IReadOnlyCollection<string> Neighbors) : IParsable<ValveData?>
{
    public override string ToString()
    {
        return $"Index={Index}, Name={Name}, Rate={Rate}, Neighbors={Neighbors.StringJoin()}";
    }

    private static readonly Regex Regex = new("Valve (?<Valve>\\w{2}) has flow rate=(?<Rate>\\d+); tunnels? leads? to valves? (?<Neighbors>[\\w\\s\\,]+)", RegexOptions.Compiled);

    public static ValveData Parse(string s, IFormatProvider? provider)
    {
        if (!TryParse(s, provider, out var result))
            throw new Exception($"Cannot parse '{s}'");
        return result;
    }

    public static bool TryParse(string? s, IFormatProvider? provider, [NotNullWhen(true)] out ValveData? result)
    {
        result = null;
        if (string.IsNullOrEmpty(s))
            return false;
        var match = Regex.Match(s);
        if (!match.Success)
            return false;
        result = new ValveData(
            -1,
            match.Groups["Valve"].Value,
            int.Parse(match.Groups["Rate"].Value),
            match.Groups["Neighbors"].Value.Split(", ").AsReadOnly()
        );
        return true;
    }
}

public class Day16
{
    public static void Solve(IEnumerable<string> fileInput)
    {
        var valves = fileInput
            .Select((line, i) => ValveData.Parse(line, null) with { Index = i })
            .ToArray();
        var start = valves.Single(x => x.Name == "AA");

        Measure.Time(() =>
            {
                GetTotalFlowForEachState(valves, start.Index, 30).Values.Max().Dump("Part1: ");
            })
            .Dump("Elapsed: ");
        
        Measure.Time(() =>
        {
            var solutionsMask = GetTotalFlowForEachState(valves, start.Index, maxTime: 26);
            solutionsMask.SelectMany(elfSolution => solutionsMask.Select(elephantSolution => (elfSolution, elephantSolution)))
                .Where(x => (x.elfSolution.Key & x.elephantSolution.Key) == 0)
                .Max(x => x.elfSolution.Value + x.elephantSolution.Value)
                .Dump("Part2: ");
        }).Dump("Elapsed: ");

    }

    private static Dictionary<long, int> GetTotalFlowForEachState(ValveData[] valves, int startIndex, int maxTime)
    {
        var nameToIndex = valves.ToDictionary(x => x.Name, x => x.Index);
        var neighborsIndices = valves.ToDictionary(valve => valve.Index,
            valve => valve.Neighbors.Select(name => nameToIndex[name]).ToArray());
        var states = new Dictionary<State, int>
        {
            { new(0, startIndex), 0 }
        };
        foreach (var _ in Enumerable.Range(1, maxTime))
        {
            var next = new Dictionary<State, int>();
            foreach (var group in states.GroupBy(kv => kv.Key.OpenMask))
            {
                var openMask = group.Key;
                var thisTickDrop = valves.Select(valve => openMask.HasBit(valve.Index) ? valve.Rate : 0).Sum();
                foreach (var ((_, valveIndex), totalDrop) in group)
                {
                    var valve = valves[valveIndex];
                    var nextTotalDrop = totalDrop + thisTickDrop;
                    if (!openMask.HasBit(valveIndex) && valve.Rate > 0)
                    {
                        var state = new State(openMask.SetBit(valveIndex), valveIndex);
                        next[state] = Math.Max(next.GetValueOrDefault(state), nextTotalDrop);
                    }

                    foreach (var state in neighborsIndices[valve.Index].Select(ind => new State(openMask, ind)))
                        next[state] = Math.Max(next.GetValueOrDefault(state), nextTotalDrop);
                }
            }

            states = next;
        }

        return states
            .GroupBy(kv => kv.Key.OpenMask)
            .ToDictionary(g => g.Key, g => g.Max(kv => kv.Value));
    }
}