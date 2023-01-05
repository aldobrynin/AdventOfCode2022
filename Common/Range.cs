namespace Common;

public record Range(int From, int To)
{
    public IEnumerable<int> All() => Enumerable.Range(From, To - From + 1);
    public Range Grow(int n) => new Range(From - n, To + n);
    public bool Contains(int v) => From <= v && v <= To;
}