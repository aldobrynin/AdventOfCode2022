using System.Collections;
using System.Numerics;
using Spectre.Console;

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

    public static T Product<T>(this IEnumerable<T> source) where T : INumber<T> =>
        source.Aggregate(T.One, (x, y) => x * y);

    public static TK Product<T, TK>(this IEnumerable<T> source, Func<T, TK> selector) where TK : INumber<TK> =>
        source.Aggregate(TK.One, (x, y) => x * selector(y));

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

    public static T Part1<T>(this T source) => SubmitAnswer(source, 1);
    public static T Part2<T>(this T source) => SubmitAnswer(source, 2);

    private static T SubmitAnswer<T>(T source, int part) {
        ArgumentNullException.ThrowIfNull(source);
        var answerString = source.ToString();
        ArgumentException.ThrowIfNullOrWhiteSpace(answerString);

        var answer = part switch {
            1 => AoCContext.Answers.Part1,
            2 => AoCContext.Answers.Part2,
            _ => throw new ArgumentOutOfRangeException(nameof(part), part, "Part number is out of range")
        };
        var hasCorrectAnswer = !string.IsNullOrWhiteSpace(answer);
        var isCorrectAnswer = answer == answerString.Trim();
        var format = "Part{0}: {1}";
        format = hasCorrectAnswer switch {
            true when !isCorrectAnswer => $"[red]{format} ❌[/]",
            true when isCorrectAnswer => $"[green]{format} ✅[/]",
            _ => format
        };

        AnsiConsole.MarkupLine(format, part, Markup.Escape(answerString));
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

    public static int ToInt(this string source) => int.Parse(source);
    public static long ToLong(this string source) => long.Parse(source);

    public static int[] ToIntArray(this string source, string separators = " ,;") =>
        source.Split(separators.ToCharArray(), StringSplitOptions.RemoveEmptyEntries)
            .Select(int.Parse)
            .ToArray();

    public static long[] ToLongArray(this string source, string separators = " ,;") =>
        source.Split(separators.ToCharArray(), StringSplitOptions.RemoveEmptyEntries)
            .Select(long.Parse)
            .ToArray();


    public static string ToHumanTimeString(this TimeSpan span, int significantDigits = 3) {
        var format = "G" + significantDigits;
        return span.TotalMilliseconds < 1000
            ? span.TotalMilliseconds.ToString(format) + " milliseconds"
            : span.TotalSeconds < 60
                ? span.TotalSeconds.ToString(format) + " seconds"
                : span.TotalMinutes < 60
                    ? span.TotalMinutes.ToString(format) + " minutes"
                    : span.TotalHours < 24
                        ? span.TotalHours.ToString(format) + " hours"
                        : span.TotalDays.ToString(format) + " days";
    }


    public static T Reduce<T>(this IEnumerable<T> source, Func<T, T, T> reducer) {
        using var enumerator = source.GetEnumerator();
        if (!enumerator.MoveNext()) throw new ArgumentException("Sequence contains no elements");
        var accumulator = enumerator.Current;
        while (enumerator.MoveNext()) accumulator = reducer(accumulator, enumerator.Current);
        return accumulator;
    }

    public static IEnumerable<(T A, T B)> Pairs<T>(this IEnumerable<T> source) {
        var list = source.ToList();
        return list.SelectMany((a, ind) => list.Skip(ind).Select(b => (a, b)));
    }

    public static IEnumerable<(T Element, int Index)> WithIndex<T>(this IEnumerable<T> source) {
        return source.Select((x, i) => (x, i));
    }

    public static IEnumerable<TNumber> RunningSum<T, TNumber>(this IEnumerable<T> source, Func<T, TNumber> selector)
        where TNumber : INumber<TNumber> {
        var sum = TNumber.Zero;
        foreach (var item in source) {
            sum += selector(item);
            yield return sum;
        }
    }

    public static IEnumerable<T> Repeat<T>(this IEnumerable<T> source, int times) {
        return Enumerable.Range(0, times).SelectMany(_ => source);
    }

    public static IEnumerable<T> RangeTo<T>(this T from, T endInclusive) where T : INumber<T> {
        return RangeTo(from, endInclusive, from > endInclusive ? -T.One : T.One);
    }

    public static IEnumerable<T> RangeUntil<T>(this T from, T endExclusive) where T : INumber<T> {
        var step = from > endExclusive ? -T.One : T.One;
        return RangeTo(from, endExclusive + step, step);
    }

    public static IEnumerable<T> RangeTo<T>(this T from, T endInclusive, T step) where T : INumber<T> {
        var isPositiveStep = T.IsPositive(step);
        if (step == T.Zero) ArgumentOutOfRangeException.ThrowIfZero(step, nameof(step));
        if (isPositiveStep) ArgumentOutOfRangeException.ThrowIfGreaterThan(from, endInclusive, nameof(from));
        else ArgumentOutOfRangeException.ThrowIfLessThan(from, endInclusive, nameof(from));

        for (var i = from; isPositiveStep ? i <= endInclusive : i >= endInclusive; i += step)
            yield return i;
    }
}