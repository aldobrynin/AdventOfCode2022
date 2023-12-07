namespace Solution.Day20;

public class Day20
{
    public static void Solve(IEnumerable<string> input)
    {
        var encryptedMessage = input.Select(long.Parse).ToArray();
        DecryptMessage(encryptedMessage, cycles: 1, multiplier: 1)
            .Dump("Part1: ");

        DecryptMessage(encryptedMessage, cycles: 10, multiplier: 811589153)
            .Dump("Part2: ");
    }

    private static long DecryptMessage(IReadOnlyList<long> input, int cycles, long multiplier)
    {
        var targetIndices = new[] { 1000, 2000, 3000 };
        var nodes = CreateList(input, multiplier);
        for (var index = 0; index < cycles * nodes.Count; index++)
        {
            var node = nodes[index % nodes.Count];
            var shift = Math.Abs(node.Value) % (nodes.Count - 1);
            for (var i = 0; i < shift; i++)
                ShiftRight(node.Value > 0 ? node : node.Prev);
        }

        return nodes
            .Single(x => x.Value == 0)
            .EndlessEnumeration()
            .Take(targetIndices.Max() + 1)
            .Where((_, ind) => targetIndices.Contains(ind))
            .Sum();
    }

    private static IReadOnlyList<Node> CreateList(IReadOnlyList<long> input, long multiplier)
    {
        var list = new List<Node>(input.Count);
        foreach (var index in input.Indices())
        {
            var node = new Node(input[index] * multiplier);
            if (index > 0)
            {
                node.Prev = list[index - 1];
                list[index - 1].Next = node;
            }
            list.Add(node);
        }

        list[0].Prev = list[^1];
        list[^1].Next = list[0];
        return list;
    }

    private static void ShiftRight(Node node)
    {
        var prevNode = node.Prev;
        var nextNode = node.Next;
        var nextNextNode = nextNode.Next;

        prevNode.Next = nextNode;
        nextNode.Prev = prevNode;
        nextNode.Next = node;
        node.Prev = nextNode;
        node.Next = nextNextNode;
        nextNextNode.Prev = node;
    }

    private class Node
    {
        public Node(long value) => Value = value;

        public long Value { get; }
        public Node Next { get; set; } = null!;
        public Node Prev { get; set; } = null!;

        public IEnumerable<long> EndlessEnumeration()
        {
            for (var current = this;; current = current.Next)
                yield return current.Value;
            // ReSharper disable once IteratorNeverReturns
        }
    }
}