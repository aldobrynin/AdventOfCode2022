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

        var index = SearchHelpers.BinarySearchUpperBound(part1FallenBytes, bytes.Length, IsExitReachable);
        $"{bytes[index].X},{bytes[index].Y}".Part2();

        bool IsExitReachable(int fallenBytesCount) => FindDistance(fallenBytesCount) > 0;

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
