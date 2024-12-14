using Common.AoC;
using Range = Common.Range;

namespace AoC2024.Day14;

public static partial class Day14 {
    public record Robot(V Position, V Velocity) {
        public static Robot Parse(string line) {
            var parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            var position = V.Parse(parts[0][2..]);
            var velocity = V.Parse(parts[1][2..]);
            return new Robot(position, velocity);
        }
    }

    public static void Solve(IEnumerable<string> input) {
        var robots = input.Select(Robot.Parse).ToArray();

        var size = AoCContext.IsSample ? new V(11, 7) : new(101, 103);
        var fullRange = Range2d.From(Range.FromStartAndEnd(0, size.X), Range.FromStartAndEnd(0, size.Y));

        var part1State = GetStateAtTime(robots, time: 100).ToArray();
        ToQuadrants(fullRange)
            .Product(q => part1State.Count(x => q.Contains(x.Position)))
            .Part1();

        if (AoCContext.IsSample) {
            return;
        }

        string[] pattern = [
            "###############################",
        ];

        var patternMap = Map.From(pattern);
        var coordinatesToCheck = fullRange
            .All()
            .Where(v => fullRange.Contains(v + new V(patternMap.LastRowIndex, patternMap.LastRowIndex)))
            .ToArray();

        var treeTime = Enumerable.Range(0, 100_000)
            .Select(time => (Time: time, State: GetStateAtTime(robots, time)))
            .First(x => ContainsPattern(x.State))
            .Time
            .Part2();

        if (AoCContext.IsSample) {
            BuildMap(treeTime).Dump();
        }

        IEnumerable<Robot> GetStateAtTime(Robot[] initial, int time) {
            return initial
                .Select(robot => robot with { Position = (robot.Position + robot.Velocity * time).Mod(size) });
        }

        bool ContainsPattern(IEnumerable<Robot> currentState) {
            var positions = currentState.Select(x => x.Position).ToHashSet();
            return coordinatesToCheck
                .AsParallel()
                .Any(start => patternMap.Coordinates().All(offset => positions.Contains(start + offset)));
        }

        Map<char> BuildMap(int time) {
            var freq = GetStateAtTime(robots, time).Select(x => x.Position).ToHashSet();
            var map = new Map<char>((int)size.X, (int)size.Y);
            foreach (var v in map.Coordinates()) {
                map[v] = freq.Contains(v) ? '#' : '.';
            }

            return map;
        }
    }

    private static Range2d<long>[] ToQuadrants(Range2d<long> range) {
        var midX = range.X.Length;
        var midY = range.Y.Length;
        var halfXRange = range.X / 2;
        var halfYRange = range.Y / 2;
        var xRanges = new[] { halfXRange, halfXRange + midX / 2 + 1 };
        var yRanges = new[] { halfYRange, halfYRange + midY / 2 + 1 };
        return xRanges.SelectMany(x => yRanges.Select(y => Range2d.From(x, y))).ToArray();
    }
}
