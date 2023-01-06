using System.Text.RegularExpressions;
using Common;
using Range = Common.Range;

namespace AoC2021;

public class Day22
{
    private record Procedure(bool Enable, Range3d Range)
    {
        private static readonly Regex Regex = new(@"[\d\-]+", RegexOptions.Compiled);

        public static Procedure Parse(string line)
        {
            var state = line.Split(' ', 2)[0];
            var captures = Regex.Matches(line)
                .Select(x => x.Value)
                .Select(int.Parse)
                .ToArray();
            var range = new Range3d(
                new Range(captures[0], captures[1]),
                new Range(captures[2], captures[3]),
                new Range(captures[4], captures[5])
            );
            return new(state == "on", range);
        }
    }

    public static void Solve(IEnumerable<string> input)
    {
        var ranges = input.Select(Procedure.Parse).ToArray();
        var initRange = new Range(-50, 50);
        var range3d = new Range3d(initRange, initRange, initRange);

        var enabledSet = new HashSet<V3>();
        foreach (var (enable, range) in ranges)
        {
            foreach (var v in range.Intersect(range3d)?.All() ?? Enumerable.Empty<V3>())
            {
                if (enable) enabledSet.Add(v);
                else enabledSet.Remove(v);
            }
        }

        enabledSet.Count.Dump("Part1: ");
    }
}

