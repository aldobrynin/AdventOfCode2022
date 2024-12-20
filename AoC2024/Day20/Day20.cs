using Common.AoC;
using Range = Common.Range;

namespace AoC2024.Day20;

public static partial class Day20 {
    public static void Solve(IEnumerable<string> input) {
        var map = Map.From(input);
        var start = map.FindFirst('S');
        var end = map.FindFirst('E');

        var distancesFromStart = map
            .Bfs((_, to) => IsWalkable(map[to]), start)
            .ToDictionary(x => x.State, x => x);
        var distanceWithoutCheat = distancesFromStart[end].Distance;
        var minSaveByCheating = AoCContext.IsSample ? 64 : 100;

        GetCheats(2)
            .Select(DistanceWithCheat)
            .Count(x => distanceWithoutCheat - x >= minSaveByCheating)
            .Part1();

        GetCheats(20)
            .Select(DistanceWithCheat)
            .Count(x => distanceWithoutCheat - x >= minSaveByCheating)
            .Part2();

        long DistanceWithCheat((V From, V To) cheat) {
            var distanceToCheat = distancesFromStart[cheat.From].Distance;
            var distanceFromCheat = distanceWithoutCheat - distancesFromStart[cheat.To].Distance;
            var cheatDistance = cheat.From.DistTo(cheat.To);
            return distanceToCheat + distanceFromCheat + cheatDistance;
        }

        IEnumerable<(V From, V To)> GetCheats(int distance) {
            return map
                .FindAll(IsWalkable)
                .SelectMany(from => WithinDistance(from, distance).Where(to => IsWalkable(map[to])).Select(to => (from, to)));
        }

        IEnumerable<V> WithinDistance(V from, int distance) {
            var range = Range.FromStartAndEndInclusive<long>(-distance, distance);
            return Range2d.From(range + from.X, range + from.Y)
                .All()
                .Where(map.Contains)
                .Where(x => x.DistTo(from) <= distance);
        }

        bool IsWalkable(char c) => c is not '#';
    }
}
