using System.Collections;

namespace Common;

public static class Extensions {
    public static V Find<T>(this T[][] map, T value) => map.FindAll(value).First();

    public static T Get<T>(this T[][] map, V pos) {
        if (pos.IsInRange(map))
            return map[pos.Y][pos.X];
        throw new ArgumentOutOfRangeException(nameof(pos), pos, "Index out of range");
    }

    public static T Get<T>(this T[,] map, V pos) {
        if (pos.IsInRange(map))
            return map[pos.Y, pos.X];
        throw new ArgumentOutOfRangeException(nameof(pos), pos, "Index out of range");
    }

    public static void Set<T>(this T[][] map, V pos, T value) {
        if (pos.IsInRange(map))
            map[pos.Y][pos.X] = value;
        else
            throw new ArgumentOutOfRangeException(nameof(pos), pos, "Index out of range");
    }

    public static void Set<T>(this T[,] map, V pos, T value) {
        if (pos.IsInRange(map))
            map[pos.Y, pos.X] = value;
        else
            throw new ArgumentOutOfRangeException(nameof(pos), pos, "Index out of range");
    }

    public static IEnumerable<V> FindAll<T>(this T[][] map, T value) {
        for (var y = 0; y < map.Length; y++)
        for (var x = 0; x < map[y].Length; x++) {
            if (Equals(map[y][x], value))
                yield return new(x, y);
        }
    }

    public static IEnumerable<V> FindAll<T>(this Map<T> map, T value) {
        return map.Coordinates().Where(v => Equals(map[v], value));
    }

    public static int Product(this IEnumerable<int> source) => source.Aggregate(1, (x, y) => x * y);

    public static int Product<T>(this IEnumerable<T> source, Func<T, int> selector) =>
        source.Aggregate(1, (x, y) => x * selector(y));

    public static long Product<T>(this IEnumerable<T> source, Func<T, long> selector) =>
        source.Aggregate(1L, (x, y) => x * selector(y));

    public static long Product(this IEnumerable<long> source) => source.Aggregate(1L, (x, y) => x * y);

    public static T Dump<T>(this T source, string? message = null, Func<T, object>? transform = null,
        string? separator = ",") {
        var printObject = transform == null ? source : transform(source);
        if (message != null) Console.Out.Write(message);
        if (printObject is IEnumerable enumerable and not string) {
            var isFirst = true;
            foreach (var s in Stringify(enumerable)) {
                if (!isFirst) Console.Out.Write(separator);
                Console.Out.Write(s);
                isFirst = false;
            }
        }
        else Console.Out.Write(printObject);

        Console.Out.WriteLine();
        return source;
    }

    public static bool HasBit(this long v, int bitIndex) => (v & (1L << bitIndex)) != 0;
    public static long SetBit(this long v, int bitIndex) => v | (1L << bitIndex);


    public static IEnumerable<int> Indices<T>(this IEnumerable<T> source) {
        return source.Select((_, i) => i);
    }

    public static T[][] Transpose<T>(this T[][] source) {
        var copy = new T[source[0].Length][];
        for (int x = 0; x < source[0].Length; x++) {
            copy[x] = new T[source.Length];
            for (int y = 0; y < source.Length; y++) {
                copy[x][y] = source[y][x];
            }
        }

        return copy;
    }

    public static IEnumerable<V> Coordinates<T>(this T[][] map) {
        for (var y = 0; y < map.Length; y++)
        for (var x = 0; x < map[y].Length; x++)
            yield return new V(x, y);

    }

    public static T[,] DumpMap<T>(this T[,] source, string? message = null, Func<T, object>? transform = null) {
        if (message != null)
            Console.WriteLine(message);
        for (int i = 0; i < source.GetLength(0); i++) {
            for (int j = 0; j < source.GetLength(1); j++) {
                var print = transform == null ? source[i, j] : transform(source[i, j]);
                Console.Write(print);
            }

            Console.WriteLine();
        }

        return source;
    }

    private static IEnumerable<string?> Stringify(IEnumerable enumerable) {
        foreach (var item in enumerable) {
            yield return item?.ToString();
        }
    }

    public static string StringJoin<T>(this IEnumerable<T> source, string? separator = ",") {
        return string.Join(separator, source);
    }

    public static int Mod(this int value, int divisor) => (value % divisor + divisor) % divisor;


    public static IEnumerable<T[]> Rows<T>(this T[][] source) {
        return source.Select((_, y) => source.ElementAt(y));
    }

    public static IEnumerable<T[]> Columns<T>(this T[][] source) {
        for (var x = 0; x < source[0].Length; x++)
            yield return source.Rows().Select(r => r[x]).ToArray();
    }

    public static IEnumerable<IReadOnlyList<string>> SplitBy(
        this IEnumerable<string> source,
        Func<string, bool> isSeparator) {
        var list = new List<string>();
        foreach (var s in source) {
            if (isSeparator(s)) {
                yield return list;
                list = new List<string>();
            }
            else list.Add(s);
        }

        if (list.Count > 0)
            yield return list;
    }

    public static HashSet<T> IntersectAll<T>(this IEnumerable<IEnumerable<T>> source) {
        HashSet<T> set = null!;
        foreach (var current in source) {
            if (set is null) set = current.ToHashSet();
            else set.IntersectWith(current);
        }

        return set;
    }

    public static Dictionary<T, int> CountFrequency<T>(this IEnumerable<T> source) where T : notnull {
        return source.GroupBy(x => x).ToDictionary(x => x.Key, x => x.Count());
    }
}