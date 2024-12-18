using Common.AoC;
using Range = Common.Range;

namespace AoC2024.Day18;

public static partial class Day18 {
    public static void Solve(IEnumerable<string> input) {
        var bytes = input.Select(V.Parse).ToArray();

        var gridSize = AoCContext.IsSample ? 6 : 70;
        var fromStartAndEndInclusive = Range.FromStartAndEndInclusive(0, gridSize);
        var range = Range2d.From(
            fromStartAndEndInclusive,
            fromStartAndEndInclusive
        );

        var start = V.Zero;
        var end = new V(gridSize, gridSize);

        var part1FallenBytes = AoCContext.IsSample ? 12 : 1024;
        FindDistance(part1FallenBytes).Part1();

        int left = 0, right = bytes.Length;
        while (left < right - 1) {
            var mid = left + (right - left) / 2;
            if (FindDistance(mid) == -1) right = mid;
            else left = mid;
        }

        $"{bytes[left].X},{bytes[left].Y}".Part2();

        int FindDistance(int fallenBytesCount) {
            var fallenBytes = bytes.Take(fallenBytesCount).ToHashSet();
            return SearchHelpers
                .Bfs(state => state.Area4()
                    .Where(range.Contains)
                    .Where(next => !fallenBytes.Contains(next)), initialStates: start)
                .FirstOrDefault(x => x.State == end)
                ?.Distance ?? -1;
        }
    }
}
