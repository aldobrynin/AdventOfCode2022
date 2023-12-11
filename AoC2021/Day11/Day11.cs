namespace AoC2021.Day11;

public partial class Day11
{
    public static void Solve(IEnumerable<string> input)
    {
        var array = input.Select(line => line.Select(c => c - '0').ToArray())
            .ToArray();
        Simulate(new Map<int>(array)).Take(100).Sum().Part1();

        Simulate(new Map<int>(array))
            .Select((f, i) => (FlashesCount: f, Step: i + 1))
            .First(x => x.FlashesCount == array.Length * array.Length)
            .Step
            .Part2();
    }

    private static IEnumerable<int> Simulate(Map<int> map)
    {
        var step = 0;
        while (step < 1000)
        {
            var flashed = new HashSet<V>();
            var queue = new Queue<V>();
            foreach (var v in map.Coordinates())
            {
                map[v] = (map[v] + 1) % 10;
                if (map[v] == 0 && flashed.Add(v)) queue.Enqueue(v);
            }

            while (queue.TryDequeue(out var item ))
            {
                foreach (var v in map.Area8(item).Where(x => !flashed.Contains(x)))
                {
                    map[v] = (map[v] + 1) % 10;
                    if (map[v] == 0 && flashed.Add(v)) queue.Enqueue(v);
                }
            }

            step++;
            yield return flashed.Count;
        }
    }
}

