using Common.AoC;
using Range = Common.Range;

namespace AoC2024.Day14;

public static partial class Day14 {
    public record Robot(V Position, V Velocity) {
        public Robot Move(int time, V mapSize) => this with { Position = (Position + Velocity * time).Mod(mapSize) };

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

        Enumerable.Range(0, 10_000)
            .First(time => ContainsPattern(GetStateAtTime(robots, time)))
            .Part2();
        IEnumerable<Robot> GetStateAtTime(Robot[] initial, int time) => initial.Select(x => x.Move(time, size));

        bool ContainsPattern(IEnumerable<Robot> currentState) {
            var positions = currentState.Select(x => x.Position).ToHashSet();
            return positions.Count(x => x.Area4().Count(positions.Contains) > 2) > robots.Length / 4;
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
