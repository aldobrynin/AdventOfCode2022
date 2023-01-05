namespace Common;

public record Range2d(Range X, Range Y)
{
    public IEnumerable<V> All() => Y.All().SelectMany(y => X.All().Select(x => new V(x, y)));
    public Range2d Grow(int n) => new(X.Grow(n), Y.Grow(n));
    public bool Contains(V v) => X.Contains(v.X) && Y.Contains(v.Y);
}