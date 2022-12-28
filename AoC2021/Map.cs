using Common;

namespace AoC2021;

public class Map<T>
{
    private readonly T[][] _arr;
    private readonly int _sizeX;
    private readonly int _sizeY;

    public Map(T[][] arr)
    {
        _sizeX = arr[0].Length;
        _sizeY = arr.Length;
        _arr = new T[_sizeY][];
        for (var y = 0; y < _sizeY; y++)
        {
            _arr[y] = new T[_sizeX];
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
}