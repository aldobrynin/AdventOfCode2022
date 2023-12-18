namespace AoC2022.Day12;

public partial class Day12
{
    public static void Solve(IEnumerable<string> input) {
        var map = Map.From(input);
        var start = map.FindFirst('S');
        var end = map.FindFirst('E');
        map[start] = 'a';
        map[end] = 'z';

        bool CanClimb(V from, V to) => map[to] - map[from] <= 1;

        map.Bfs(CanClimb, start)
            .First(x => x.State == end)
            .Distance
            .Part1();

        map.Bfs(CanClimb, map.FindAll('a').ToArray())
            .First(x => x.State == end)
            .Distance
            .Part2();
    }
}