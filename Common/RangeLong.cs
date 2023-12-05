namespace Common;

public record RangeLong(long From, long To) {
    public long Length => To - From + 1;
    public bool IsEmpty() => From > To;
    public bool Contains(long value) => From <= value && value <= To;
    public bool Contains(RangeLong other) => Contains(other.From) && Contains(other.To);

    public RangeLong? Intersect(RangeLong other) {
        return HasIntersection(other) ? new RangeLong(Math.Max(From, other.From), Math.Min(To, other.To)) : null;
    }

    public RangeLong IntersectOrThrow(RangeLong other) {
        return Intersect(other) ?? throw new InvalidOperationException($"Range {this} doesn't intersect with {other}");
    }

    public bool HasIntersection(RangeLong other) {
        return Contains(other.From) || Contains(other.To) || other.Contains(From) || other.Contains(To);
    }

    public static RangeLong FromStartAndLength(long start, long length) => new(start, start + length - 1);
    public IEnumerable<RangeLong> Subtract(RangeLong other) => Subtract(new[] { other });

    public IEnumerable<RangeLong> Subtract(IEnumerable<RangeLong> other) {
        var start = From;
        foreach (var range in other.Where(HasIntersection).OrderBy(x => x.From)) {
            var newRange = new RangeLong(start, range.From - 1);
            if (!newRange.IsEmpty()) yield return newRange;

            start = range.To + 1;
        }

        if (start <= To) yield return new RangeLong(start, To);
    }

    public static RangeLong operator +(RangeLong r, long offset) => new(r.From + offset, r.To + offset);

    public override string ToString() => $"[{From};{To}]";
}