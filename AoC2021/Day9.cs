using Common;

namespace AoC2021;

public class Day9
{
    public static void Solve(IEnumerable<string> input)
    {
        var map =
            new Map<int>(input.Select(line => line.Select(c => c - '0').ToArray())
                .ToArray());

        map.Coordinates()
            .Where(v => map.Area4(v).All(n => map[n] > map[v]))
            .Sum(v => map[v] + 1)
            .Dump("Part1: ");

        var basins = new List<int>();
        var visited = new HashSet<V>();
        foreach (var v in map.Coordinates().Where(visited.Add).Where(v => map[v] != 9))
        {
            var queue = new Queue<V>();
            queue.Enqueue(v);
            var basin = 1;
            while (queue.TryDequeue(out var item))
            {
                foreach (var next in map.Area4(item).Where(visited.Add).Where(n => map[n] != 9))
                {
                    queue.Enqueue(next);
                    basin++;
                }
            }

            basins.Add(basin);
        }

        basins.OrderDescending()
            .Take(3)
            .Product()
            .Dump("Part2: ");
    }
}