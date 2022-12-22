using Common;

namespace Solution;

public class Day8
{
    public static void Solve(IEnumerable<string> input)
    {
        var map = input.Select(s => s.Select(c => c - '0').ToArray()).ToArray();

        EnumerateTrees(map)
            .Count(tree => V.Directions4.Any(dir => CanReachEndOfMap(map, tree, dir)))
            .Dump("Part1: ");

        EnumerateTrees(map)
            .Max(tree => V.Directions4.Product(dir => GetLastVisibleTree(map, tree, dir).DistTo(tree)))
            .Dump("Part2: ");
    }

    private static bool CanReachEndOfMap(int[][] map, V tree, V direction)
    {
        if (IsOnBorder(map, tree))
            return true;
        var visibleTree = GetLastVisibleTree(map, tree, direction);
        return IsOnBorder(map, visibleTree) && map.Get(visibleTree) < map.Get(tree);
    }

    private static bool IsOnBorder(int[][] map, V tree) =>
        tree.X == 0 || tree.Y == 0 || tree.X == map[0].Length - 1 || tree.Y == map.Length - 1;
    
    private static V GetLastVisibleTree(int[][] map, V fromTree, V direction)
    {
        var tree = fromTree + direction;
        while (tree.IsInRange(map) && map.Get(tree) < map.Get(fromTree))
            tree += direction;
        return tree.IsInRange(map) ? tree : tree - direction;
    }

    private static IEnumerable<V> EnumerateTrees(int[][] map) =>
        map.Indices()
            .SelectMany(y => map[y].Indices().Select(x => new V(x, y)));
}