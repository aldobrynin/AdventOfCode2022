namespace AoC2022.Day14;

public partial class Day14
{
    public static void Solve(IEnumerable<string> input)
    {
        var lines = input
            .Select(x => x.Split(" -> ").Select(V.Parse).ToArray())
            .ToArray();
        var sandSource = new V(500, 0);
        var maxY = lines.SelectMany(x => x.Select(v => v.Y)).Max() + 2;

        DropSand(sandSource, maxY, lines)
            .TakeWhile(s => s.Y != maxY)
            .Count()
            .Part1();
        
        var maxX = lines.SelectMany(x => x.Select(v => v.X)).Max();
        var floorLine = new[] { new V(0, maxY), new V(maxX * 2, maxY) };
        (DropSand(sandSource, maxY, lines.Append(floorLine))
                .TakeWhile(s => s != sandSource)
                .Count() + 1)
            .Part2();
    }
    
    static IEnumerable<V> DropSand(V sandSource, long maxY, IEnumerable<V[]> blockLines)
    {
        var map = new HashSet<V>();
        
        foreach (var line in blockLines)
            for (var i = 1; i < line.Length; i++)
                map.UnionWith(line[i - 1].LineTo(line[i]));

        while (true)
        {
            var sand = sandSource;
            do
            {
                var move = Move(sand);
                if (move == sand)
                    break;
                sand = move;
            } while (true);

            yield return sand;
            map.Add(sand);
        }
        
        V Move(V pos)
        {
            var nextPos = MoveTowards(pos, V.Down, 1);
            if (nextPos != pos)
                return nextPos;
            nextPos = MoveTowards(pos, V.Down + V.Left);
            if (nextPos != pos)
                return nextPos;
            pos = nextPos;
            return MoveTowards(pos, V.Down + V.Right);
        }

        V MoveTowards(V pos, V dir, int movesLimit = 1)
        {
            var result = pos;
            var movesCount = 0;
            while ((result + dir).Y <= maxY && !map.Contains(result + dir) && movesCount++ < movesLimit)
            {
                result += dir;
            }

            return result;
        }
    }
}