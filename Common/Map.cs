namespace Common;

public class Map<T>
{
    private readonly T[][] _arr;

    public int SizeX { get; }

    public int SizeY { get; }

    public Map(int sizeX, int sizeY)
    {
        SizeX = sizeX;
        SizeY = sizeY;
        _arr = new T[SizeY][];
        for (var y = 0; y < SizeY; y++) _arr[y] = new T[SizeX];
    }

    public Map(T[][] arr)
    {
        SizeX = arr[0].Length;
        SizeY = arr.Length;
        _arr = new T[SizeY][];
        for (var y = 0; y < SizeY; y++)
        {
            _arr[y] = new T[SizeX];
            Array.Copy(arr[y], _arr[y], arr[y].Length);
        }
    }

    public IEnumerable<V> Coordinates() => _arr.Coordinates();

    public T this[V key]
    {
        get => _arr.Get(key);
        set => _arr.Set(key, value);
    }

    public IEnumerable<V> Area4(V v)
    {
        return v.Area4().Where(x => x.IsInRange(_arr));
    }
    
    public IEnumerable<V> Area8(V v)
    {
        return v.Area8().Where(x => x.IsInRange(_arr));
    }

    public void Print()
    {
        foreach (var row in _arr)
        {
            foreach (var el in row) Console.Write(el);
            Console.WriteLine();
        }
    }

    public Map<T> Clone() => new(_arr);
}

public class Map
{
    public static Map<T> From<T>(T[][] source) => new(source);
}