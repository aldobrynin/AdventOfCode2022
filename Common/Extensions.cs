using System.Collections;
using System.Text;

namespace Common;

public static class Extensions
{
    public static V Find<T>(this T[][] map, T value) => map.FindAll(value).First();

    public static T Get<T>(this T[][] map, V pos)
    {
        if (pos.IsInRange(map))
            return map[pos.Y][pos.X];
        throw new ArgumentOutOfRangeException(nameof(pos), pos, "Index out of range");
    }

    public static void Set<T>(this T[][] map, V pos, T value)
    {
        if (pos.IsInRange(map))
            map[pos.Y][pos.X] = value;
        else
            throw new ArgumentOutOfRangeException(nameof(pos), pos, "Index out of range");
    }

    public static IEnumerable<V> FindAll<T>(this T[][] map, T value)
    {
        for (var y = 0; y < map.Length; y++)
        for (var x = 0; x < map[y].Length; x++)
        {
            if (Equals(map[y][x], value))
                yield return new(x, y);
        }
    }

    public static int Product(this IEnumerable<int> source) => source.Aggregate(1, (x, y) => x * y);
    public static long Product(this IEnumerable<long> source) => source.Aggregate(1L, (x, y) => x * y);

    public static T Dump<T>(this T source, string? message = null, Func<T, object>? transform = null)
    {
        var printObject = transform == null ? source : transform(source);
        var sb = new StringBuilder();
        if (message != null)
            sb.Append(message);
        if (printObject is IEnumerable enumerable and not string)
            sb.AppendJoin(",", Stringify(enumerable));
        else
            sb.Append(printObject);
        Console.WriteLine(sb.ToString());
        return source;
    }

    public static bool HasBit(this long v, int bitIndex) => (v & (1L << bitIndex)) != 0;
    public static long SetBit(this long v, int bitIndex) => v | (1L << bitIndex);


    public static IEnumerable<int> Indices<T>(this IEnumerable<T> source)
    {
        return source.Select((_, i) => i);
    }

    public static T[,] DumpMap<T>(this T[,] source, string? message = null, Func<T, object>? transform = null)
    {
        if (message != null)
            Console.WriteLine(message);
        for (int i = 0; i < source.GetLength(0); i++)
        {
            for (int j = 0; j < source.GetLength(1); j++)
            {
                var print = transform == null ? source[i, j] : transform(source[i, j]);
                Console.Write(print);
            }
            
            Console.WriteLine();
        }

        return source;
    }

    private static IEnumerable<string?> Stringify(IEnumerable enumerable)
    {
        foreach (var item in enumerable)
        {
            yield return item?.ToString();
        }
    }

    public static string StringJoin<T>(this IEnumerable<T> source, string? separator = ",")
    {
        return string.Join(separator, source);
    }
    
    public static IEnumerable<BfsState> Bfs<T>(this T[][] map, CanMove canMove, params V[] initial)
    {
        var visited = initial.ToHashSet();
        var queue = new Queue<BfsState>();
        foreach (var v in initial)
            queue.Enqueue(new BfsState(v, 0));
    
        while (queue.TryDequeue(out var item))
        {
            yield return item;
            var result = item;
            var next = V.Directions4
                .Select(x => x + result.Pos)
                .Where(x => x.IsInRange(map))
                .Where(v => canMove(result.Pos, v))
                .Where(x => !visited.Contains(x));
            foreach (var n in next)
            {
                visited.Add(n);
                queue.Enqueue(new BfsState(n, item.Steps + 1));
            }
        }
    }
}

public record BfsState(V Pos, int Steps);
public delegate bool CanMove(V from, V to);