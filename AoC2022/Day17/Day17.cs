using System.Text;

namespace AoC2022.Day17;

public class Shape
{
    private Shape(IEnumerable<V> points)
    {
        Points = points.ToHashSet();
    }

    public HashSet<V> Points { get; }

    public int MinY => Points.Min(v => v.Y);
    public int MaxY => Points.Max(v => v.Y);

    public int MinX => Points.Min(v => v.X);
    public int MaxX => Points.Max(v => v.X);

    public static Shape Parse(string input)
    {
        return new Shape(input.Split("\n")
            .Reverse()
            .SelectMany((line, y) => line.Select((c, x) => (V: new V(x, y), c)))
            .Where(x => x.c == '#')
            .Select(x => x.V));
    }

    public static Shape operator +(Shape shape, V offset)
    {
        return new(shape.Points.Select(p => p + offset));
    }

    public override string ToString()
    {
        var sb = new StringBuilder();
        for (var y = MaxY; y >= MinY; y--)
        {
            var row = Enumerable.Range(MinX, MaxX - MinX + 1)
                .Select(x => new V(x, y))
                .Select(v => Points.Contains(v) ? '#' : '.')
                .ToArray();
            sb.AppendLine(new string(row));
        }

        return sb.ToString();
    }
}

public class GameField
{
    private readonly int _caveWidth;
    private readonly HashSet<V> _rockCells;

    public GameField(int maxRocksCount, int caveWidth)
    {
        _caveWidth = caveWidth;
        _rockCells = new HashSet<V>(maxRocksCount);
        _rockCells.UnionWith(Enumerable.Range(0, caveWidth).Select(x => new V(x, 0)));
    }

    public void AddRock(Shape rock) => _rockCells.UnionWith(rock.Points);
    private bool IsWithinCave(V v) => v.X >= 0 && v.X < _caveWidth && v.Y > 0;
    public bool HasCollisions(Shape rock) => !rock.Points.All(v => IsWithinCave(v) && !_rockCells.Contains(v));

    public void Print(Shape? fallingRock)
    {
        var minX = _rockCells.Min(x => x.X);
        var maxX = _rockCells.Max(x => x.X);
        var minY = _rockCells.Min(x => x.Y);
        var maxY = _rockCells.Max(x => x.Y) + 7;
        var currentShape = fallingRock?.Points ?? new HashSet<V>();
        Console.WriteLine(new string('=', _caveWidth));
        for (var y = maxY; y >= minY; y--)
        {
            var row = Enumerable.Range(minX, maxX - minX + 1)
                .Select(x => new V(x, y))
                .Select(v => _rockCells.Contains(v) ? '#' : currentShape.Contains(v) ? '@' : '.')
                .ToArray();
            Console.WriteLine(new string(row));
        }
    }
}

public partial class Day17
{
    private static readonly Shape[] _shapes = new[]
    {
        @"####",

        @".#.
###
.#.",

        @"..#
..#
###",

        @"#
#
#
#",

        @"##
##"
    }.Select(Shape.Parse).ToArray();

    public static void Solve(IEnumerable<string> fileInput)
    {
        const int caveWidth = 7;
        var wind = fileInput.Single().Select(x => x == '>' ? V.Right : V.Left).ToArray();
        var move = 0;

        var maxRocksCount = Math.Max(wind.Length, 2022);
        var heightDiffs = new List<int>(maxRocksCount);
        var gameMap = new GameField(maxRocksCount, caveWidth);
        var newRockSpawnOffset = new V(2, 4);
        var maxY = 0;

        foreach (var currentRock in Enumerable.Range(0, maxRocksCount).Select(i => _shapes[i % _shapes.Length]))
        {
            var maxYBeforeRock = maxY;
            var rockSpawnOffset = new V(0, maxYBeforeRock) + newRockSpawnOffset;
            var fallingRock = currentRock + rockSpawnOffset;

            do TryMove(ref fallingRock, wind[move++ % wind.Length]);
            while (TryMove(ref fallingRock, V.Up));

            maxY = Math.Max(maxY, fallingRock.MaxY);
            gameMap.AddRock(fallingRock);

            heightDiffs.Add(maxY - maxYBeforeRock);
            if (heightDiffs.Count == 2022)
                maxY.Part1();
        }

        var (start, period) = FindSequencePeriod(heightDiffs);
        const long targetCount = 1000000000000L;
        var tailLength = (targetCount - start) % period;
        var headSum = heightDiffs.Take(start).Sum(x => x);
        var periodsCount = (targetCount - start - tailLength) / period;
        var tailSum = heightDiffs.Skip(start).Take((int)tailLength).Sum(x => x);
        var total = headSum
                    + heightDiffs.Skip(start).Take(period).Sum(x => x) * periodsCount
                    + tailSum;
        total.Part2();

        bool TryMove(ref Shape shape, V dir)
        {
            var nextShape = shape + dir;
            if (gameMap.HasCollisions(nextShape)) return false;
            shape = nextShape;
            return true;
        }
    }

    private static (int Start, int Period) FindSequencePeriod(IEnumerable<int> list)
    {
        var span = new ReadOnlySpan<char>(list.Select(x => (char)(x + 48)).ToArray());
        var patternStart = span.Length - 30;
        var pattern = span[patternStart..^1];
        var start = span[20..].IndexOf(pattern) + 20;
        var nextStart = span[(start + pattern.Length)..].IndexOf(pattern) + start + pattern.Length;
        var period = nextStart - start;
        return (start, period);
    }
}