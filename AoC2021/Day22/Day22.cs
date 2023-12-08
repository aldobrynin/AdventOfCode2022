using System.Text.RegularExpressions;
using Range = Common.Range;

namespace AoC2021.Day22;

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
                Range<int>.FromStartAndEndInclusive(captures[0], captures[1]),
                Range<int>.FromStartAndEndInclusive(captures[2], captures[3]),
                Range<int>.FromStartAndEndInclusive(captures[4], captures[5])
            );
            return new(state == "on", range);
        }
    }

    public static void Solve(IEnumerable<string> input)
    {
        var ranges = input.Select(Procedure.Parse).ToArray();
        var initRange = Range.FromStartAndEndInclusive(-50, 50);
        var initRange3d = new Range3d(initRange, initRange, initRange);

        var cubes = new HashSet<Range3d>();
        foreach (var (enable, range) in ranges)
        {
            var intersectingCubes = cubes.Where(c => c.Intersects(range)).ToArray();
            if (enable)
            {
                var newCubes = intersectingCubes.Aggregate(new[] { range },
                    (cur, next) => cur.SelectMany(s => s.Subtract(next)).ToArray()
                );
                cubes.UnionWith(newCubes);
            }
            else
            {
                foreach (var cube in intersectingCubes)
                {
                    cubes.Remove(cube);
                    cubes.UnionWith(cube.Subtract(range));
                }
            }
        }

        cubes.Select(c => c.Intersect(initRange3d))
            .Sum(c => c == null ? 0 : 1L * c.X.Length * c.Y.Length * c.Z.Length)
            .Dump("Part1: ");

        cubes.Sum(c => 1L * c.X.Length * c.Y.Length * c.Z.Length)
            .Dump("Part2: ");
    }
}

