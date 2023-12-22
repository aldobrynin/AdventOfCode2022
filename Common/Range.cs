using System.Collections;
using System.Numerics;

namespace Common;

public record Range<T>(T From, T To) : IEnumerable<T> where T : INumber<T> {
    public static Range<T> Empty => FromStartAndLength(T.Zero, T.Zero);
    public T Length => To - From;
    public T ToInclusive => To - T.One;
    public bool IsEmpty() => From >= To;
    public bool Contains(T value) => From <= value && value < To;
    public bool Contains(Range<T> other) => From <= other.From && other.To <= To;
    public bool Overlaps(Range<T> other) => T.Max(From, other.From) <= T.Min(To, other.To);

    public Range<T> Grow(T value) => new(From - value, To + value);

    public Range<T>? Intersect(Range<T> other) {
        return HasIntersection(other) ? FromStartAndEnd(T.Max(From, other.From), T.Min(To, other.To)) : null;
    }

    public Range<T> IntersectOrThrow(Range<T> other) {
        return Intersect(other) ?? throw new InvalidOperationException($"Range {this} doesn't intersect with {other}");
    }

    public bool HasIntersection(Range<T> other) {
        return Contains(other.From) || Contains(other.ToInclusive) ||
               other.Contains(From) || other.Contains(ToInclusive);
    }

    public static Range<T> FromStartAndLength(T start, T length) => new(start, start + length);
    public static Range<T> FromStartAndEnd(T start, T endExclusive) => new(start, endExclusive);

    public (Range<T>? Left, Range<T>? Right) Subtract(Range<T> other) {
        var left = From < other.From ? FromStartAndEnd(From, T.Min(other.From, To)) : null;
        var right = To > other.To ? FromStartAndEnd(T.Max(From, other.To), To) : null;
        return (left, right);
    }

    public IEnumerable<Range<T>> Subtract(IEnumerable<Range<T>> other) {
        var current = this;
        foreach (var range in other.OrderBy(x => x.From)) {
            var (left, right) = current.Subtract(range);
            if (left is not null) yield return left;
            if (right is null) yield break;
            current = right;
        }

        yield return current;
    }

    public static Range<T> operator +(Range<T> r, T offset) => new(r.From + offset, r.To + offset);
    public static Range<T> operator -(Range<T> r, T offset) => new(r.From - offset, r.To - offset);

    public override string ToString() => $"[{From};{To})";

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<T> GetEnumerator() {
        for (var i = From; i < To; i += T.One) yield return i;
    }
}

public static class Range {
    public static Range<T> FromStartAndLength<T>(T start, T length) where T : INumber<T> =>
        Range<T>.FromStartAndLength(start, length);

    public static Range<T> FromStartAndEnd<T>(T start, T endExclusive) where T : INumber<T> =>
        Range<T>.FromStartAndEnd(start, endExclusive);

    public static Range<T> FromStartAndEndInclusive<T>(T start, T endInclusive) where T : INumber<T> =>
        Range<T>.FromStartAndEnd(start, endInclusive + T.One);
}