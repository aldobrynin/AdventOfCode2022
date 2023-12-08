using System.Collections;
using System.Numerics;

namespace Common;

public record Range<T>(T From, T To) : IEnumerable<T> where T : INumber<T> {
    public T Length => To - From + T.One;
    public bool IsEmpty() => From > To;
    public bool Contains(T value) => From <= value && value <= To;
    public bool Contains(Range<T> other) => Contains(other.From) && Contains(other.To);

    public Range<T> Grow(T value) => new(From - value, To + value);

    public Range<T>? Intersect(Range<T> other) {
        return HasIntersection(other) ? new(T.Max(From, other.From), T.Min(To, other.To)) : null;
    }

    public Range<T> IntersectOrThrow(Range<T> other) {
        return Intersect(other) ?? throw new InvalidOperationException($"Range {this} doesn't intersect with {other}");
    }

    public bool HasIntersection(Range<T> other) {
        return Contains(other.From) || Contains(other.To) || other.Contains(From) || other.Contains(To);
    }

    public static Range<T> FromStartAndLength(T start, T length) => new(start, start + length - T.One);
    public static Range<T> FromStartAndEndInclusive(T start, T endInclusive) => new(start, endInclusive);

    public static Range<T> FromStartAndEndExclusive(T start, T endExclusive) =>
        FromStartAndEndInclusive(start, endExclusive - T.One);

    public IEnumerable<Range<T>> Subtract(Range<T> other) => Subtract(new[] { other });

    public IEnumerable<Range<T>> Subtract(IEnumerable<Range<T>> other) {
        var start = From;
        foreach (var range in other.Where(HasIntersection).OrderBy(x => x.From)) {
            var newRange = FromStartAndEndExclusive(start, range.From);
            if (!newRange.IsEmpty()) yield return newRange;

            start = range.To + T.One;
        }

        if (start <= To) yield return new(start, To);
    }

    public static Range<T> operator +(Range<T> r, T offset) => new(r.From + offset, r.To + offset);



    public override string ToString() => $"[{From};{To}]";

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<T> GetEnumerator() {
        for (var i = From; i <= To; i += T.One) yield return i;
    }
}

public static class Range {
    public static Range<T> FromStartAndLength<T>(T start, T length) where T : INumber<T> =>
        Range<T>.FromStartAndLength(start, length);
    
    public static Range<T> FromStartAndEndInclusive<T>(T start, T endInclusive) where T : INumber<T> =>
        Range<T>.FromStartAndEndInclusive(start, endInclusive);
}