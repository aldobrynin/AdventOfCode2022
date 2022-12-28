using Common;

namespace AoC2021;

public class Day6
{
    public static void Solve(IEnumerable<string> input)
    {
        var ages = input.Single().Split(',').Select(int.Parse).ToArray();
        Simulate(ages).ElementAt(79).Dump("Part1: ");
        Simulate(ages).ElementAt(255).Dump("Part2: ");
    }
    
    private static IEnumerable<long> Simulate(IEnumerable<int> initialAges)
    {
        var fishPerDay = initialAges.GroupBy(x => x)
            .ToDictionary(x => x.Key, x => x.LongCount());
        var day = 0;
        while (day++ < 1000)
        {
            var nextSpawn = new Dictionary<int, long>(fishPerDay.Count);
            foreach (var (age, count) in fishPerDay)
            {
                if (age == 0)
                {
                    AddOrIncrease(nextSpawn, 6, count);
                    AddOrIncrease(nextSpawn, 8, count);
                }
                else
                    AddOrIncrease(nextSpawn, age - 1, count);
            }

            fishPerDay = nextSpawn;
            yield return fishPerDay.Sum(x => x.Value);
            // fish.LongLength.Dump($"After {day:00} days:");
        }
    }

    private static void AddOrIncrease(IDictionary<int, long> dict, int key, long value)
    {
        if (!dict.TryGetValue(key, out var currentValue))
            dict[key] = value;
        else dict[key] = value + currentValue;
    }
}

