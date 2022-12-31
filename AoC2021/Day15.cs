using Common;

namespace AoC2021;

public class Day15
{
    public static void Solve(IEnumerable<string> input)
    {
        var map = new Map<int>(input.Select(line => line.Select(c => c - '0').ToArray()).ToArray());
        var start = new V(0, 0);
        FindPath(start, map)
            .First(x => x.Position == new V(map.SizeX - 1, map.SizeY - 1))
            .RiskAmount
            .Dump("Part1: ");

        var largeMap = EnlargeMap(map, factor: 5);
        FindPath(start, largeMap)
            .First(x => x.Position == new V(largeMap.SizeX - 1, largeMap.SizeY - 1))
            .RiskAmount
            .Dump("Part2: ");
    }

    private static Map<int> EnlargeMap(Map<int> map, int factor)
    {
        var newMap = new Map<int>(map.SizeX * factor, map.SizeY * factor);
        foreach (var v in newMap.Coordinates())
        {
            var value = map[v.Mod(map.SizeY, map.SizeX)] + v.Y / map.SizeY + v.X / map.SizeX;
            newMap[v] = (value - 1) % 9 + 1;
        }

        return newMap;
    }

    private static IEnumerable<(V Position, int RiskAmount)> FindPath(V start, Map<int> map)
    {
        var visited = new Dictionary<V, int>();
        var queue = new PriorityQueue<V, int>();
        queue.Enqueue(start, 0);
        visited.Add(start, 0);

        while (queue.TryDequeue(out var item, out var riskValue))
        {
            yield return (item, riskValue);

            foreach (var (neighbor, neighborRisk) in map.Area4(item).Select(v => (v, riskValue + map[v])))
            {
                if (visited.GetValueOrDefault(neighbor, int.MaxValue) <= neighborRisk) continue;
                queue.Enqueue(neighbor, neighborRisk);
                visited[neighbor] = neighborRisk;
            }
        }
    }
}