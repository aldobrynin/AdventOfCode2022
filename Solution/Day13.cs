using System.Text.Json;
using Common;

namespace Solution;

class Comparer : IComparer<JsonElement>
{
    public int Compare(JsonElement x, JsonElement y)
    {
        if (x.ValueKind == JsonValueKind.Number && y.ValueKind == JsonValueKind.Number)
            return Comparer<int>.Default.Compare(x.GetInt32(), y.GetInt32());
        var a = x.ValueKind == JsonValueKind.Number ? new[] { x } : x.EnumerateArray().ToArray();
        var b = y.ValueKind == JsonValueKind.Number ? new[] { y } : y.EnumerateArray().ToArray();
        for (var i = 0; i < Math.Min(a.Length, b.Length); i++)
        {
            var compare = Compare(a[i], b[i]);
            if (compare != 0)
                return compare;
        }

        return Comparer<int>.Default.Compare(a.Length, b.Length);
    }
}

public class Day13
{
    public static void Solve(IEnumerable<string> input)
    {
        var comparer = new Comparer();

        var lines = input
            .Where(x => string.IsNullOrWhiteSpace(x) == false)
            .ToArray();

        Solve1().Sum().Dump("Part1: ");
        Solve2().Product().Dump("Part2: ");

        IEnumerable<int> Solve1()
        {
            var index = 0;
            foreach (var pair in lines.Select(s => JsonDocument.Parse(s).RootElement).Chunk(2))
            {
                index++;
                if (comparer.Compare(pair[0], pair[1]) <= 0)
                    yield return index;
            }
        }

        IEnumerable<int> Solve2()
        {
            var dividers = new []{"[[2]]", "[[6]]"}
                .Select(x => JsonDocument.Parse(x).RootElement)
                .ToArray();
            var packets = lines
                .Select(x => JsonDocument.Parse(x).RootElement)
                .Concat(dividers)
                .ToArray();
            Array.Sort(packets, comparer);
            return dividers.Select(divider => Array.BinarySearch(packets, divider, comparer) + 1);
        }
    }
}