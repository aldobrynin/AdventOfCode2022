using System.Collections;
using System.Numerics;
using Common.AoC;
using Spectre.Console;

namespace Common;

public static class Extensions {
    public static T Get<T>(this T[][] map, V pos) {
        if (pos.IsInRange(map))
            return map[pos.Y][pos.X];
        throw new ArgumentOutOfRangeException(nameof(pos), pos, "Index out of range");
    }

    public static void Set<T>(this T[][] map, V pos, T value) {
        if (pos.IsInRange(map))
            map[pos.Y][pos.X] = value;
        else
            throw new ArgumentOutOfRangeException(nameof(pos), pos, "Index out of range");
    }

    public static IEnumerable<V> FindAll<T>(this Map<T> map, T value) => map.FindAll(v => Equals(v, value));

    public static IEnumerable<V> FindAll<T>(this Map<T> map, Func<T, bool> predicate) => map.Coordinates()
        .Where(v => predicate(map[v]));

    public static V FindFirst<T>(this Map<T> map, T value) => map.FindFirst(v => Equals(v, value));
    public static V FindFirst<T>(this Map<T> map, Func<T, bool> predicate) => map.Coordinates().First(v => predicate(map[v]));

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
        format = (hasCorrectAnswer, isCorrectAnswer) switch {
            (false, _) => format,
            (_, false) => $"[red]{format} ❌  (expected: {answer})[/]",
            (_, true) => $"[green]{format} ✅[/]",
        };

        AnsiConsole.MarkupLine(format, part, Markup.Escape(answerString));
        return source;
    }

    public static bool HasBit(this long v, int bitIndex) => (v & (1L << bitIndex)) != 0;
    public static long SetBit(this long v, int bitIndex) => v | (1L << bitIndex);
    public static long UnsetBit(this long v, int bitIndex) => v & ~(1L << bitIndex);

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

    public static IEnumerable<IReadOnlyList<T>> SplitBy<T>(
        this IEnumerable<T> source,
        Func<T, bool> isSeparator) {
        var list = new List<T>();
        foreach (var s in source) {
            if (isSeparator(s)) {
                yield return list;
                list = new List<T>();
            }
            else list.Add(s);
        }

        if (list.Count > 0)
            yield return list;
    }

    public static IEnumerable<IReadOnlyList<T>> PartitionBy<T>(this IEnumerable<T> source,
        Func<T, bool> isStartOfPartition) {
        var list = new List<T>();
        foreach (var s in source) {
            if (isStartOfPartition(s)) {
                yield return list;
                list = [s];
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

    public static Dictionary<T, long> CountFrequency<T>(this IEnumerable<T> source) where T : notnull {
        return source.GroupBy(x => x).ToDictionary(x => x.Key, x => x.LongCount());
    }

    public static Dictionary<TValue, long> CountFrequency<TSource, TValue>(this IEnumerable<TSource> source,
        Func<TSource, TValue> selector) where TValue : notnull {
        return source.GroupBy(selector).ToDictionary(x => x.Key, x => x.LongCount());
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

    public static IEnumerable<(T Element, int Index)> WithIndex<T>(this IEnumerable<T> source) {
        return source.Select((x, i) => (x, i));
    }

    public static IEnumerable<TResult> Running<T, TResult>(this IEnumerable<T> source, TResult initial,
        Func<TResult, T, TResult> selector) {
        var current = initial;
        foreach (var item in source) {
            current = selector(current, item);
            yield return current;
        }
    }

    public static IEnumerable<TResult> RunningFold<T, TResult>(this IEnumerable<T> source, TResult initial,
        Func<TResult, T, TResult> selector) {
        var current = initial;
        yield return current;
        foreach (var item in source) {
            current = selector(current, item);
            yield return current;
        }
    }

    public static IEnumerable<TNumber> RunningSum<T, TNumber>(this IEnumerable<T> source, Func<T, TNumber> selector)
        where TNumber : INumber<TNumber> {
        return source.Running(TNumber.Zero, (acc, value) => acc + selector(value));
    }

    public static IEnumerable<T> Repeat<T>(this IEnumerable<T> source, int times) {
        return Enumerable.Range(0, times).SelectMany(_ => source);
    }

    public static IEnumerable<T> RangeTo<T>(this T from, T endInclusive) where T : INumber<T> {
        return RangeTo(from, endInclusive, from > endInclusive ? -T.One : T.One);
    }

    public static IEnumerable<T> RangeUntil<T>(this T from, T endExclusive) where T : INumber<T> {
        var step = from > endExclusive ? -T.One : T.One;
        return RangeTo(from, endExclusive - step, step);
    }

    public static IEnumerable<T> RangeTo<T>(this T from, T endInclusive, T step) where T : INumber<T> {
        var isPositiveStep = T.IsPositive(step);
        if (step == T.Zero) ArgumentOutOfRangeException.ThrowIfZero(step, nameof(step));
        if (isPositiveStep) ArgumentOutOfRangeException.ThrowIfGreaterThan(from, endInclusive, nameof(from));
        else ArgumentOutOfRangeException.ThrowIfLessThan(from, endInclusive, nameof(from));

        for (var i = from; isPositiveStep ? i <= endInclusive : i >= endInclusive; i += step)
            yield return i;
    }

    public static IEnumerable<T> Pipe<T>(this IEnumerable<T> source, Action<T> action) {
        foreach (var item in source) {
            action(item);
            yield return item;
        }
    }

    public static IEnumerable<T> Flatten<T>(this IEnumerable<IEnumerable<T>> source) => source.SelectMany(x => x);

    public static IEnumerable<(T Prev, T Next)> ZipWithNext<T>(this IEnumerable<T> source) =>
        source.ZipWithNext((prev, next) => (prev, next));

    public static IEnumerable<TResult> ZipWithNext<T, TResult>(this IEnumerable<T> source,
        Func<T, T, TResult> selector) {
        using var enumerator = source.GetEnumerator();
        if (!enumerator.MoveNext()) yield break;
        var prev = enumerator.Current;
        while (enumerator.MoveNext()) {
            yield return selector(prev, enumerator.Current);
            prev = enumerator.Current;
        }
    }

    public static T MaxOrDefault<T>(this IEnumerable<T> source, T defaultValue) where T : IComparable<T> {
        using var enumerator = source.GetEnumerator();
        if (!enumerator.MoveNext()) return defaultValue;
        var max = enumerator.Current;
        while (enumerator.MoveNext()) {
            if (enumerator.Current.CompareTo(max) > 0) max = enumerator.Current;
        }

        return max;
    }

    public static TValue MaxOrDefault<T, TValue>(this IEnumerable<T> source, Func<T, TValue> selector,
        TValue defaultValue) where TValue : IComparable<TValue> {
        return source.Select(selector).MaxOrDefault(defaultValue);
    }

    public static T Lcm<T>(this IEnumerable<T> source) where T : INumber<T> => source.Aggregate(MathHelpers.Lcm);

    public static TValue GetOrAdd<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, Func<TKey, TValue> valueFactory) {
        if (!dictionary.TryGetValue(key, out var value)) {
            dictionary[key] = value = valueFactory(key);
        }

        return value;
    }

    public static IEnumerable<T[]> SlidingWindow<T>(this IEnumerable<T> source, int windowSize) {
        var window = new Queue<T>(windowSize);
        foreach (var item in source) {
            window.Enqueue(item);
            if (window.Count == windowSize) {
                yield return window.ToArray();
                window.Dequeue();
            }
        }
    }

    public static IEnumerable<T> GenerateSequence<T>(this T seed, Func<T, T> generator) {
        var current = seed;
        while (true) {
            yield return current;
            current = generator(current);
        }
        // ReSharper disable once IteratorNeverReturns
    }

    public static TResult Apply<T, TResult>(this T source, Func<T, TResult> selector) => selector(source);


    public static int IndexOf<T>(this IEnumerable<T> sequence, T[] target, IEqualityComparer<T>? equalityComparer = null) {
        var targetIndex = 0;
        equalityComparer ??= EqualityComparer<T>.Default;
        foreach (var (value, index) in sequence.WithIndex()) {
            if (!equalityComparer.Equals(value, target[targetIndex])) {
                targetIndex = 0;
                continue;
            }

            if (++targetIndex == target.Length) {
                return index - target.Length + 1;
            }
        }

        return -1;
    }

    public static IEnumerable<T> TakeUntil<T>(this IEnumerable<T> source, Func<T, bool> predicate) {
        foreach (var item in source) {
            yield return item;
            if (predicate(item)) break;
        }
    }

    public static (T Min, T Max) MinMax<T>(this IEnumerable<T> source) where T : IComparable<T> {
        using var enumerator = source.GetEnumerator();
        if (!enumerator.MoveNext()) throw new ArgumentException("Sequence contains no elements");
        var min = enumerator.Current;
        var max = enumerator.Current;
        while (enumerator.MoveNext()) {
            var current = enumerator.Current;
            if (current.CompareTo(min) < 0) min = current;
            if (current.CompareTo(max) > 0) max = current;
        }

        return (min, max);
    }
}
