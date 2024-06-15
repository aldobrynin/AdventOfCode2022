namespace AoC2019.Day20;

public static partial class Day20 {
    private record struct Portal(string Name, bool IsOuter);

    private record struct State(Portal Portal, int Level = 0);

    public static void Solve(IEnumerable<string> input) {
        var map = Map.From(input);
        var adjMap = BuildAdjacencyMap(map);

        var start = new State(new Portal("AA", IsOuter: true));
        var end = new State(new Portal("ZZ", IsOuter: true));

        SearchHelpers
            .Dijkstra(current => GetNextStates(current, recursive: false), initialStates: start)
            .First(x => x.State == end)
            .Distance
            .Part1();

        SearchHelpers
            .Dijkstra(current => GetNextStates(current, recursive: true), initialStates: start)
            .First(x => x.State == end)
            .Distance
            .Part2();

        IEnumerable<SearchPathItem<State>> GetNextStates(SearchPathItem<State> current, bool recursive = false) {
            var (portal, level) = current.State;
            return adjMap[portal]
                .Select(x => (x.Neighbor, x.Distance, Level: recursive ? GetLevel(level, portal, x.Neighbor) : level))
                .Where(x => x.Level >= 0)
                .Select(x => current.Next(new State(x.Neighbor, x.Level), x.Distance));
        }

        int GetLevel(int level, Portal from, Portal to) {
            if (from.Name == to.Name) return from.IsOuter ? level - 1 : level + 1;
            return level;
        }
    }

    private static Dictionary<Portal, (Portal Neighbor, int Distance)[]> BuildAdjacencyMap(Map<char> map) {
        var portals = map
            .Coordinates()
            .Where(v => map[v] == '.' && map.Area4(v).Any(n => char.IsLetter(map[n])))
            .Select(v => {
                var diff = map.Area4(v).Single(n => char.IsLetter(map[n])) - v;
                var name = diff == V.Right || diff == V.Down
                    ? $"{map[v + diff]}{map[v + diff + diff]}"
                    : $"{map[v + diff + diff]}{map[v + diff]}";
                var isOuter = v.X == 2 || v.Y == 2 || v.X == map.SizeX - 3 || v.Y == map.SizeY - 3;
                return (new Portal(name, isOuter), v);
            }).ToDictionary(x => x.Item2, x => x.Item1);

        var adjMap = new Dictionary<Portal, (Portal Neighbor, int Distance)[]>();
        foreach (var (v, portal) in portals) {
            adjMap[portal] = map.Bfs((from, to) => (from == v || !portals.ContainsKey(from)) && map[to] == '.', v)
                .Where(x => x.State != v && portals.ContainsKey(x.State))
                .Select(x => (portals[x.State], x.Distance))
                .Concat(portal.Name is "AA" or "ZZ"
                    ? Array.Empty<(Portal, int)>()
                    : [(portal with { IsOuter = !portal.IsOuter }, 1)])
                .ToArray();
        }

        return adjMap;
    }
}