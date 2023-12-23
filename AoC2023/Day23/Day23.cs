namespace AoC2023.Day23;
using AdjacencyMap = Dictionary<V, (V Next, int Distance)[]>;

public static partial class Day23 {
    public static void Solve(IEnumerable<string> input) {
        var map = Map.From(input);

        FindLongestPath(map, CanMoveWithSlopes).Part1();
        FindLongestPath(map, CanMove).Part2();

        bool CanMoveWithSlopes(V from, V to) => map[from] == '.' ? map[to] != '#' : from + V.FromArrow(map[from]) == to;
        bool CanMove(V from, V to) => map[to] != '#';
    }

    private static int FindLongestPath(Map<char> map, CanMove canMove) {
        var start = map.TopBorder().Single(v => map[v] == '.');
        var end = map.BottomBorder().Single(v => map[v] == '.');
        var adjacencyMap = BuildAdjacencyMap(map, start, end, canMove);

        var nodeToIndex = adjacencyMap.WithIndex().ToDictionary(x => x.Element.Key, x => x.Index);
        var indexedMap = adjacencyMap.ToDictionary(
            x => nodeToIndex[x.Key],
            x => x.Value.Select(n => (Next: nodeToIndex[n.Next], n.Distance)).ToArray()
        );
        var startIdx = nodeToIndex[start];
        var endIdx = nodeToIndex[end];

        return LongestPathToEnd(startIdx);

        int LongestPathToEnd(int current, long visited = 0L) {
            if (current == endIdx) return 0;
            return indexedMap[current].Where(x => !visited.HasBit(x.Next))
                .MaxOrDefault(x => x.Distance + LongestPathToEnd(x.Next, visited.SetBit(x.Next)), int.MinValue);
        }
    }

    private static AdjacencyMap BuildAdjacencyMap(Map<char> map, V start, V end, CanMove canMove) {
        var nodes = map.Coordinates()
            .Where(from => map[from] != '#' && map.Area4(from).Count(to => canMove(from, to)) > 2)
            .Append(start)
            .Append(end)
            .ToHashSet();

        return nodes.ToDictionary(
            node => node,
            node => map
                .Bfs((from, to) => !IsOtherNode(node, from) && canMove(from, to), node)
                .Where(x => IsOtherNode(node, x.State))
                .Select(x => (x.State, x.Distance))
                .ToArray()
        );

        bool IsOtherNode(V source, V node) => node != source && nodes.Contains(node);
    }
}