using Common;

namespace Solution;

public class Day12
{
    public static void Solve(IEnumerable<string> input)
    {
        var map = input
            .Select(x => x.ToCharArray())
            .ToArray();
        var start = map.Find('S');
        var end = map.Find('E');
        map.Set(start, 'a');
        map.Set(end, 'z');

        bool CanClimb(V from, V to) => map.Get(to) - map.Get(from) <= 1;

        map.Bfs(CanClimb, start)
            .First(x => x.Pos == end)
            .Dump("Part1: ");

        map.Bfs(CanClimb, map.FindAll('a').ToArray())
            .First(x => x.Pos == end)
            .Dump("Part2: ");
    }
}