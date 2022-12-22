public record V(int X, int Y)
{
    public static V Left = new V(-1, 0);
    public static V Right = new V(1, 0);
    public static V Up = new V(0, 1);
    public static V Down = new V(0, -1);

    public bool IsInRange<T>(T[][] map)
    {
        return X >= 0 && X < map[0].Length && Y >= 0 && Y < map.Length;
    }
    
    public bool IsInRange<T>(T[,] map)
    {
        return X >= 0 && X < map.GetLength(1) && Y >= 0 && Y < map.GetLength(0);
    }

    public static V Parse(string s)
    {
        var tokens = s.Split(new []{',', ' '}, 2);
        return new V(int.Parse(tokens[0]), int.Parse(tokens[1]));
    }

    public static V[] Directions4 => new[]
    {
        Left,
        Up,
        Right,
        Down,
    };
    
    public static V Zero => new V(0, 0);

    public int DistTo(V other) => Math.Abs(X - other.X) + Math.Abs(Y - other.Y);
    public V Abs() => new V(Math.Abs(X), Math.Abs(Y));

    public int CDistTo(V other) => Math.Max(Math.Abs(other.X - X), Math.Abs(other.Y - Y));
    
    public int MLen =>  Math.Abs(X) + Math.Abs(Y);
    public int CLen =>  Math.Max(Math.Abs(X), Math.Abs(Y));

    public IEnumerable<V> LineTo(V other)
    {
        var dir = (other - this).Signum();
        for (var point = this; point != other; point += dir)
            yield return point;

        yield return other;
    }

    public V Rotate(int degrees)
    {
        double angle = Math.PI * degrees / 180.0;
        (double sinAngle, double cosAngle) = Math.SinCos(angle);

        var x = (int)(X * cosAngle - Y * sinAngle);
        var y = (int)(X * sinAngle + Y * cosAngle);
        return new(x, y);
    }

    public override string ToString() => $"[{X},{Y}]";

    public static V operator +(V a, V b) => new V(a.X + b.X, a.Y + b.Y);
    public static V operator -(V a, V b) => new V(a.X - b.X, a.Y - b.Y);
    public static V operator -(V a) => new V(-a.X, -a.Y);
    public static V operator *(V a, int k) => new V(k * a.X, k * a.Y);
    public static V operator *(int k, V a) => new V(k * a.X, k * a.Y);
    public static V operator /(V a, int k) => new V(a.X / k, a.Y / k);

    public V Signum() => new V(Math.Sign(X), Math.Sign(Y));
}