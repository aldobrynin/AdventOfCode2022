namespace AoC2022.Day08;

public partial class Day08
{
    public static void Solve(IEnumerable<string> input) {
        var map = Map.From(input.Select(s => s.Select(c => c - '0')));

        map.Coordinates()
            .Count(tree => V.Directions4.Any(dir => CanReachEndOfMap(map, tree, dir)))
            .Part1();

        map.Coordinates()
            .Max(tree => V.Directions4.Product(dir => GetLastVisibleTree(map, tree, dir).DistTo(tree)))
            .Part2();
    }

    private static bool CanReachEndOfMap(Map<int> map, V tree, V direction)
    {
        if (IsOnBorder(map, tree))
            return true;
        var visibleTree = GetLastVisibleTree(map, tree, direction);
        return IsOnBorder(map, visibleTree) && map[visibleTree] < map[tree];
    }

    private static bool IsOnBorder(Map<int> map, V tree) =>
        tree.X == 0 || tree.Y == 0 || tree.X == map.LastColumnIndex || tree.Y == map.LastRowIndex;
    
    private static V GetLastVisibleTree(Map<int> map, V fromTree, V direction)
    {
        var tree = fromTree + direction;
        while (tree.IsInRange(map) && map[tree] < map[fromTree])
            tree += direction;
        return tree.IsInRange(map) ? tree : tree - direction;
    }
}