using Common.AoC;

namespace AoC2024.Day20;

public static partial class Day20 {
    public record Cheat(V From, V To);

    public static void Solve(IEnumerable<string> input) {
        var map = Map.From(input);
        var start = map.FindFirst('S');
        var end = map.FindFirst('E');

        var distancesFromStart = map.Bfs(CanMove, start)
            .ToDictionary(x => x.State, x => x.Distance);
        var distancesFromEnd = map.Bfs(CanMove, end)
            .ToDictionary(x => x.State, x => x.Distance);
        var distanceWithoutCheat = distancesFromStart[end];
        var minSaveByCheating = AoCContext.IsSample ? 64 : 100;

        GetCheats(2)
            .Count(cheat => DistanceCutByCheat(cheat) >= minSaveByCheating)
            .Part1();

        GetCheats(20)
            .Count(cheat => DistanceCutByCheat(cheat) >= minSaveByCheating)
            .Part2();

        long DistanceCutByCheat(Cheat cheat) => distanceWithoutCheat - DistanceWithCheat(cheat);

        long DistanceWithCheat(Cheat cheat) =>
            distancesFromStart[cheat.From] + distancesFromEnd[cheat.To] + cheat.From.DistTo(cheat.To);

        IEnumerable<Cheat> GetCheats(int distance) =>
            distancesFromStart.Keys.SelectMany(from => CheatTo(from, distance).Select(to => new Cheat(from, to)));

        IEnumerable<V> CheatTo(V from, int distance) =>
            WithinDistance(from, distance)
                .Where(to => to != from)
                .Where(distancesFromStart.ContainsKey);

        bool IsWalkable(char c) => c is not '#';

        bool CanMove(V from, V to) => IsWalkable(map[to]);
    }

    private static IEnumerable<V> WithinDistance(V from, int distance) {
        for (var dy = -distance; dy <= distance; dy++)
        for (var dx = -distance + Math.Abs(dy); dx <= distance - Math.Abs(dy); dx++)
            yield return from + new V(dx, dy);
    }
}
