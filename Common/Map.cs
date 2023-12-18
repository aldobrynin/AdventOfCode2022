namespace Common;

public class Map<T> {
    private readonly T[][] _arr;

    public int SizeX { get; }

    public int SizeY { get; }

    public int LastRowIndex => SizeY - 1;
    public int LastColumnIndex => SizeX - 1;

    public Range<int> RowIndices => Range<int>.FromStartAndLength(0, SizeY);
    public Range<int> ColumnIndices => Range<int>.FromStartAndLength(0, SizeX);

    public Map(int sizeX, int sizeY) {
        SizeX = sizeX;
        SizeY = sizeY;
        _arr = new T[SizeY][];
        for (var y = 0; y < SizeY; y++) _arr[y] = new T[SizeX];
    }

    public Map(T[][] arr) {
        SizeX = arr[0].Length;
        SizeY = arr.Length;
        _arr = new T[SizeY][];
        for (var y = 0; y < SizeY; y++) {
            _arr[y] = new T[SizeX];
            Array.Copy(arr[y], _arr[y], arr[y].Length);
        }
    }

    public IEnumerable<V> Coordinates() => RowIndices.SelectMany(y => ColumnIndices.Select(x => new V(x, y)));

    public IEnumerable<V> TopBorder() => ColumnIndices.Select(x => new V(x, 0));
    public IEnumerable<V> BottomBorder() => ColumnIndices.Select(x => new V(x, SizeY - 1));
    public IEnumerable<V> LeftBorder() => RowIndices.Select(y => new V(0, y));
    public IEnumerable<V> RightBorder() => RowIndices.Select(y => new V(SizeX - 1, y));

    public IEnumerable<V> BorderCoordinates() => Coordinates()
        .Where(v => v.X == 0 || v.X == LastColumnIndex || v.Y == 0 || v.Y == LastRowIndex);

    public T this[V key] {
        get => _arr.Get(key);
        set => _arr.Set(key, value);
    }

    public T this[int row, int col] {
        get => _arr[row][col];
        set => _arr[row][col] = value;
    }

    public T GetValueOrDefault(V key, T defaultValue) => Contains(key) ? _arr.Get(key) : defaultValue;

    public IEnumerable<V> Area4(V v) => v.Area4().Where(Contains);

    public IEnumerable<V> Area8(V v) => v.Area8().Where(Contains);

    public IEnumerable<T[]> Rows() => _arr.Rows();

    public IEnumerable<T[]> Columns() => _arr.Columns();

    public IEnumerable<T[]> Borders() {
        var rows = Rows().Where((_, i) => i == 0 || i == LastRowIndex).ToArray();
        var columns = Columns().Where((_, i) => i == 0 || i == LastColumnIndex).ToArray();
        return new[] { columns[0], rows[0], columns[1], rows[1] };
    }

    public Map<T> Rotate90() {
        var newMap = new Map<T>(SizeY, SizeX);
        foreach (var coordinate in Coordinates()) {
            var newCoordinate = new V(SizeY - 1 - coordinate.Y, coordinate.X);
            newMap[newCoordinate] = this[coordinate];
        }

        return newMap;
    }

    public Map<T> Rotate180() => Rotate90().Rotate90();

    public Map<T> Rotate270() => Rotate180().Rotate90();

    public Map<T> Transpose() {
        var newMap = new Map<T>(SizeY, SizeX);
        foreach (var coordinate in Coordinates()) {
            var newCoordinate = new V(coordinate.Y, coordinate.X);
            newMap[newCoordinate] = this[coordinate];
        }

        return newMap;
    }

    public void Print() {
        foreach (var row in _arr) {
            foreach (var el in row) Console.Write(el);
            Console.WriteLine();
        }
    }

    public void PrintColored(Func<V, (string Text, ConsoleColor? Color)> transform) {
        foreach (var v in Coordinates()) {
            var before = Console.ForegroundColor;
            var (text, color) = transform(v);
            Console.ForegroundColor = color ?? before;
            Console.Write(text);
            Console.ForegroundColor = before;
            if (v.X == LastColumnIndex) Console.WriteLine();
        }
    }

    public bool Contains(V v) => 0 <= v.Y && v.Y < SizeY && 0 <= v.X && v.X < SizeX;

    public Map<T> Clone() => new(_arr);

    public override int GetHashCode() {
        var hash = new HashCode();
        foreach (var row in _arr)
        foreach (var el in row)
            hash.Add(el);

        return hash.ToHashCode();
    }
}

public static class Map {
    public static Map<T> From<T>(T[][] source) => new(source);
    public static Map<T> From<T>(IEnumerable<IEnumerable<T>> source) => new(source.Select(x => x.ToArray()).ToArray());
}