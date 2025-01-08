using System.Text.RegularExpressions;

namespace AoC2018.Day09;

public static partial class Day09 {
    public static void Solve(IEnumerable<string> input) {
        var match = Regex.Match(input.Single(),
            @"(?<players>\d+) players; last marble is worth (?<points>\d+) points");
        var players = match.Groups["players"].Value.ToInt();
        var points = match.Groups["points"].Value.ToLong();

        Solve(players, points).Part1();
        Solve(players, points * 100).Part2();
    }

    private static long Solve(int players, long points) {
        var scores = new long[players];
        var list = new LinkedList<long>();
        var current = list.AddFirst(0);

        for (var i = 1; i <= points; i++) {
            if (i % 23 == 0) {
                var marbleToRemove = NthMarbleCounterClockwise(current, 7);
                scores[i % players] += i + marbleToRemove.Value;
                current = NthMarbleClockwise(marbleToRemove, 1);
                Remove(marbleToRemove);
            }
            else current = list.AddAfter(NthMarbleClockwise(current, 1), i);
        }

        return scores.Max();

        void Remove(LinkedListNode<long> node) => list.Remove(node);

        LinkedListNode<long> NthMarbleClockwise(LinkedListNode<long> node, int n) {
            for (var i = 0; i < n; i++) node = node.Next ?? list.First!;
            return node;
        }

        LinkedListNode<long> NthMarbleCounterClockwise(LinkedListNode<long> node, int n) {
            for (var i = 0; i < n; i++) node = node.Previous ?? list.Last!;
            return node;
        }
    }
}
