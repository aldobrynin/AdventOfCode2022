namespace AoC2019.Day18;

public static partial class Day18 {
    public record struct State(long Robots, long Keys = 0);

    public static void Solve(IEnumerable<string> input) {
        var map = Map.From(input);

        Part1(map.Clone()).Part1();
        Part2(map.Clone()).Part2();
    }

    private static int Part1(Map<char> map) => FindShortestDistance(map);

    private static int Part2(Map<char> map) {
        var start = map.FindFirst('@');

        map[start] = '#';
        foreach (var v in map.Area8(start)) map[v] = '@';
        foreach (var v in map.Area4(start)) map[v] = '#';

        return FindShortestDistance(map);
    }

    private static int FindShortestDistance(Map<char> map) {
        var initial = map.FindAll('@').ToArray();
        foreach (var (element, index) in initial.WithIndex()) map[element] = (char)(index + '0');

        var (nodes, graph) = BuildGraph(map, initial);
        var tilesMap = nodes.ToDictionary(x => x.Value, x => map[x.Key]);

        var initialState = initial.Select(x => nodes[x]).Aggregate(0L, (acc, ind) => acc.SetBit(ind));

        var keysCount = map.Coordinates().Count(v => char.IsAsciiLetterLower(map[v]));
        var allKeysMask = Enumerable.Range(0, keysCount).Aggregate(0L, (acc, i) => acc.SetBit(i));
        return SearchHelpers
            .Dijkstra(GetNextState, initialStates: new State(initialState))
            .First(x => x.State.Keys == allKeysMask)
            .Distance;

        IEnumerable<SearchPathItem<State>> GetNextState(SearchPathItem<State> current) {
            return nodes.Indices()
                .Where(ind => current.State.Robots.HasBit(ind))
                .SelectMany(node => graph[node]
                    .Where(x => CanMoveTo(current.State.Keys, tilesMap[x.Neighbor]))
                    .Select(x => NextState(current, node, x))
                );
        }

        bool CanMoveTo(long keys, char tile) => !char.IsAsciiLetterUpper(tile) || keys.HasBit(tile - 'A');

        SearchPathItem<State> NextState(SearchPathItem<State> current, int from, (int Node, int Distance) next) {
            var (robots, keys) = current.State;
            var nextState = new State(robots.UnsetBit(from).SetBit(next.Node), TryAddKey(keys, tilesMap[next.Node]));
            return current.Next(nextState, next.Distance);
        }

        long TryAddKey(long keys, char tile) => char.IsAsciiLetterLower(tile) ? keys.SetBit(tile - 'a') : keys;
    }

    private static (Dictionary<V, int> Nodes, Dictionary<int, List<(int Neighbor, int Distance)>> Graph)
        BuildGraph(Map<char> map, params V[] start) {
        var graph = new Dictionary<int, List<(int Neighbor, int Distance)>>();
        var nodes = map.Bfs((_, to) => map[to] != '#', start)
            .Select(x => x.State)
            .Where(v => map[v] is not '.')
            .WithIndex()
            .ToArray();
        var nodesMap = nodes.ToDictionary(x => x.Element, x => x.Index);
        foreach (var (node, index) in nodes) {
            graph[index] = SearchHelpers
                .Bfs(state => state != node && nodesMap.ContainsKey(state)
                        ? Enumerable.Empty<V>()
                        : map.Area4(state).Where(v => map[v] != '#'),
                    initialStates: node)
                .Where(x => x.State != node && nodesMap.ContainsKey(x.State))
                .Select(x => (nodesMap[x.State], x.Distance))
                .ToList();
        }

        return (nodesMap, graph);
    }
}