namespace Common;

public record Range(int From, int To)
{
    public IEnumerable<int> All() => Enumerable.Range(From, To - From + 1);
    public Range Grow(int n) => new Range(From - n, To + n);
    public bool Contains(int v) => From <= v && v <= To;

    public Range? Intersect(Range other)
    {
        if (Contains(other.From) || Contains(other.To) || other.Contains(From) || other.Contains(To))
            return new Range(Math.Max(From, other.From), Math.Min(To, other.To));

        return null;
    }

    public override string ToString() => $"[{From};{To}]";
}