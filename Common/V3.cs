namespace Common;

public record V3(int X, int Y, int Z)
{
    public static readonly V3 Left = new(-1, 0, 0);
    public static readonly V3 Right = new(1, 0, 0);
    public static readonly V3 Up = new(0, 1, 0);
    public static readonly V3 Down = new(0, -1, 0);
    public static readonly V3 Top = new(0, 0, 1);
    public static readonly V3 Bottom = new(0, 0, -1);

    public static V3 Parse(string s)
    {
        var tokens = s.Split(new []{',', ' '}, 3, StringSplitOptions.RemoveEmptyEntries);
        return new V3(int.Parse(tokens[0]), int.Parse(tokens[1]), int.Parse(tokens[2]));
    }

    public static V3[] Directions4 => new[]
    {
        Left,
        Up,
        Right,
        Down,
    };
    
    public static V3[] Directions6 => new[]
    {
        Left,
        Up,
        Right,
        Down,
        Top,
        Bottom,
    };
    
    public static V3 Zero => new (0, 0, 0);

    public IEnumerable<V3> Neighbors6() => V3.Directions6.Select(dir => this + dir);

    public int DistTo(V3 other) => Math.Abs(X - other.X)
                                   + Math.Abs(Y - other.Y)
                                   + Math.Abs(Z - other.Z);
    public V3 Abs() => new(Math.Abs(X), Math.Abs(Y), Math.Abs(Z));

    public long CDistTo(V other) => Math.Max(Math.Abs(other.X - X), Math.Abs(other.Y - Y));

    public override string ToString() =>
        $"[{X},{Y},{Z}]";

    public static V3 operator +(V3 a, V3 b) => new(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
    public static V3 operator -(V3 a, V3 b) => new(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
    public static V3 operator -(V3 a) => new(-a.X, -a.Y, -a.Z);
    public static V3 operator *(V3 a, int k) => new(k * a.X, k * a.Y, k * a.Z);
    public static V3 operator *(int k, V3 a) => new(k * a.X, k * a.Y, k * a.Z);
    public static V3 operator /(V3 a, int k) => new(a.X / k, a.Y / k, a.Z / k);

    public V3 Signum() => new(Math.Sign(X), Math.Sign(Y), Math.Sign(Z));
}