using System.Globalization;
using System.Numerics;

namespace Common;

public record V3<T>(T X, T Y, T Z) where T : INumber<T> {
    public static readonly V3<T> Left = new(-T.One, T.Zero, T.Zero);
    public static readonly V3<T> Right = new(T.One, T.Zero, T.Zero);
    public static readonly V3<T> Up = new(T.Zero, T.One, T.Zero);
    public static readonly V3<T> Down = new(T.Zero, -T.One, T.Zero);
    public static readonly V3<T> Top = new(T.Zero, T.Zero, T.One);
    public static readonly V3<T> Bottom = new(T.Zero, T.Zero, -T.One);

    public static V3<T> Parse(string s) {
        var tokens = s.Split(new[] { ',', ' ' }, 3, StringSplitOptions.RemoveEmptyEntries);
        return new V3<T>(tokens.Select(x => T.Parse(x, CultureInfo.InvariantCulture)).ToArray());
    }

    public V3(IReadOnlyList<T> coordinates) : this(coordinates[0], coordinates[1], coordinates[2]) {
    }

    public static V3<T>[] Directions4 => new[] {
        Left,
        Up,
        Right,
        Down,
    };

    public static V3<T>[] Directions6 => new[] {
        Left,
        Up,
        Right,
        Down,
        Top,
        Bottom,
    };

    public static V3<T> Zero => new(T.Zero, T.Zero, T.Zero);

    public IEnumerable<V3<T>> Neighbors6() => Directions6.Select(dir => this + dir);

    public T DistTo(V3<T> other) => T.Abs(X - other.X) + T.Abs(Y - other.Y) + T.Abs(Z - other.Z);
    public V3<T> Abs() => new(T.Abs(X), T.Abs(Y), T.Abs(Z));
    
    public T MLen => T.Abs(X) + T.Abs(Y) + T.Abs(Z);

    public override string ToString() => $"[{X},{Y},{Z}]";

    public static V3<T> operator +(V3<T> a, V3<T> b) => new(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
    public static V3<T> operator -(V3<T> a, V3<T> b) => new(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
    public static V3<T> operator -(V3<T> a) => new(-a.X, -a.Y, -a.Z);
    public static V3<T> operator *(V3<T> a, T k) => new(k * a.X, k * a.Y, k * a.Z);
    public static V3<T> operator *(T k, V3<T> a) => new(k * a.X, k * a.Y, k * a.Z);
    public static V3<T> operator /(V3<T> a, T k) => new(a.X / k, a.Y / k, a.Z / k);

    public V3<T> Signum() {
        return new V3<T>(T.CreateChecked(T.Sign(X)), T.CreateChecked(T.Sign(Y)), T.CreateChecked(T.Sign(Z)));
    }
}