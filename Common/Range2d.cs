namespace Common;

public record Range2d(Range X, Range Y)
{
    public IEnumerable<V> All() => Y.All().SelectMany(y => X.All().Select(x => new V(x, y)));
    public Range2d Grow(int n) => new(X.Grow(n), Y.Grow(n));
    public bool Contains(V v) => X.Contains(v.X) && Y.Contains(v.Y);

    public Range2d? Intersect(Range2d other)
    {
        var intersectionX = X.Intersect(other.X);
        var intersectionY = Y.Intersect(other.Y);
        if (intersectionX != null && intersectionY != null)
            return new Range2d(intersectionX, intersectionY);
        return null;
    }
}

public record Range3d(Range X, Range Y, Range Z)
{
    public IEnumerable<V3> All() =>
        Z.All().SelectMany(z => Y.All().SelectMany(y => X.All().Select(x => new V3(x, y, z))));

    public Range3d Grow(int n) => new(X.Grow(n), Y.Grow(n), Z.Grow(n));
    public bool Contains(V3 v) => X.Contains(v.X) && Y.Contains(v.Y) && Z.Contains(v.Z);

    public Range3d? Intersect(Range3d other)
    {
        var intersectionX = X.Intersect(other.X);
        var intersectionY = Y.Intersect(other.Y);
        var intersectionZ = Z.Intersect(other.Z);
        if (intersectionX != null && intersectionY != null && intersectionZ != null)
            return new Range3d(intersectionX, intersectionY, intersectionZ);
        return null;
    }
}