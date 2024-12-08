namespace Common;

public static class CombinatoricsExtensions {
    public static IEnumerable<(T A, T B)> Pairs<T>(this IEnumerable<T> source) {
        var list = source.ToList();
        return list.SelectMany((a, ind) => list.Skip(ind).Select(b => (a, b)));
    }

    public static IEnumerable<T[]> Combinations<T>(this IEnumerable<T> source, int k) {
        var list = source.ToList();
        if (k == 0) return [];
        if (k == 1) return list.Select(x => new[] {x});
        return list.SelectMany((e, i) => list.Skip(i + 1).Combinations(k - 1).Select(c => c.Prepend(e).ToArray()));
    }

    public static IEnumerable<T[]> Variants<T>(this T[] values, int count) {
        if (count == 0) return [[]];
        if (count == 1) return values.Select(x => new[] { x });

        return Variants(values, count - 1)
            .SelectMany(x => values.Select(y => x.Append(y).ToArray()));
    }


    public static IEnumerable<T[]> Permutations<T>(this IReadOnlyCollection<T> input) {
        if (input.Count == 1) yield return input.ToArray();
        else {
            foreach (var x in input)
            foreach (var y in Permutations(input.Except(new[] { x }).ToArray()))
                yield return y.Prepend(x).ToArray();
        }
    }
}
