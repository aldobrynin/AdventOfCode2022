namespace Common;

public record Range(int From, int To)
{
    public IEnumerable<int> All() => Enumerable.Range(From, To - From + 1);
    public Range Grow(int n) => new Range(From - n, To + n);
    public bool Contains(int v) => From <= v && v <= To;
    public bool Contains(Range r) => Contains(r.From) && Contains(r.To);

    public bool IsEmpty() => From > To;

    public int Length => To - From + 1;
    public long LongLength => To - From + 1L;

    public Range? Intersect(Range other)
    {
        return Intersects(other) ? new Range(Math.Max(From, other.From), Math.Min(To, other.To)) : null;
    }

    public bool Intersects(Range other)
    {
        return Contains(other.From) || Contains(other.To) || other.Contains(From) || other.Contains(To);
    }

    public Range[] Subtract(Range other)
    {
        if (other.Contains(this))
            return Array.Empty<Range>();
        var intersect = Intersect(other);
        if (intersect == null)
            return new[] { this };
        return new[]
            {
                new Range(From, intersect.From - 1),
                new Range(intersect.To + 1, To),
            }.Where(x => !x.IsEmpty())
            .ToArray();
    }

    public override string ToString() => $"[{From};{To}]";
}