using Common.AoC;

namespace AoC2024.Day20;

public static partial class Day20 {
    public static void Solve(IEnumerable<string> input) {
        var map = Map.From(input);
        var start = map.FindFirst('S');

        var path = map
            .Bfs((_, to) => map[to] is not '#', start)
            .Select(x => x.State)
            .ToList();
        var minSaveByCheating = AoCContext.IsSample ? 64 : 100;

        GetCheats2(2).Part1();
        GetCheats2(20).Part2();

        long GetCheats2(int distance) {
            var res = 0;
            for (var i = 0; i < path.Count; i++) {
                var from = path[i];
                for (var j = i + distance + 1; j < path.Count; j++) {
                    var distanceWithoutCheat = j - i;
                    var cheatDistance = from.DistTo(path[j]);
                    var savedByCheat = distanceWithoutCheat - cheatDistance;
                    if (cheatDistance <= distance && savedByCheat >= minSaveByCheating) {
                        res++;
                    }
                }
            }

            return res;
        }
    }
}
