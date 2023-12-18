using System.Numerics;

namespace Common;

public record Range2d<T>(Range<T> X, Range<T> Y) where T : INumber<T> {
    public IEnumerable<V> All() => Y.SelectMany(y => X.Select(x => new V(int.CreateChecked(x), int.CreateChecked(y))));
    public Range2d<T> Grow(T n) => new(X.Grow(n), Y.Grow(n));
    public bool Contains(V v) => X.Contains(T.CreateChecked(v.X)) && Y.Contains(T.CreateChecked(v.Y));
    public bool Contains(Range2d<T> r) => X.Contains(r.X) && Y.Contains(r.Y);

    public bool IsEmpty() => X.IsEmpty() || Y.IsEmpty();

    public Range2d<T>? Intersect(Range2d<T> other) {
        var intersectionX = X.Intersect(other.X);
        var intersectionY = Y.Intersect(other.Y);
        if (intersectionX != null && intersectionY != null)
            return new Range2d<T>(intersectionX, intersectionY);
        return null;
    }

    public Range2d<T>[] Subtract(Range2d<T> other) {
        if (other.Contains(this))
            return Array.Empty<Range2d<T>>();
        var intersect = Intersect(other);
        if (intersect == null)
            return new[] { this };
        return new[] {
                this with { Y = Y with { To = intersect.Y.From } },
                this with { Y = Y with { From = intersect.Y.To } },
                intersect with { X = X with { To = intersect.X.From } },
                intersect with { X = X with { From = intersect.X.To } },
            }
            .Where(x => !x.IsEmpty())
            .ToArray();
    }
}

public static class Range2d {
    public static Range2d<T> From<T>(Range<T> xRange, Range<T> yRange) where T : INumber<T> {
        return new Range2d<T>(xRange, yRange);
    }
}