namespace Common;

public record Range2d(Range<int> X, Range<int> Y) {
    public IEnumerable<V> All() => Y.SelectMany(y => X.Select(x => new V(x, y)));
    public Range2d Grow(int n) => new(X.Grow(n), Y.Grow(n));
    public bool Contains(V v) => X.Contains(v.X) && Y.Contains(v.Y);
    public bool Contains(Range2d r) => X.Contains(r.X) && Y.Contains(r.Y);

    public bool IsEmpty() => X.IsEmpty() || Y.IsEmpty();

    public Range2d? Intersect(Range2d other) {
        var intersectionX = X.Intersect(other.X);
        var intersectionY = Y.Intersect(other.Y);
        if (intersectionX != null && intersectionY != null)
            return new Range2d(intersectionX, intersectionY);
        return null;
    }

    public Range2d[] Subtract(Range2d other) {
        if (other.Contains(this))
            return Array.Empty<Range2d>();
        var intersect = Intersect(other);
        if (intersect == null)
            return new[] { this };
        return new[] {
                this with { Y = Y with { To = intersect.Y.From - 1 } },
                this with { Y = Y with { From = intersect.Y.To + 1 } },
                intersect with { X = X with { To = intersect.X.From - 1 } },
                intersect with { X = X with { From = intersect.X.To + 1 } },
            }
            .Where(x => !x.IsEmpty())
            .ToArray();
    }
}