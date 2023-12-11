using System.Collections;

namespace AoC2020.Day23;

public partial class Day23 {
    public static void Solve(IEnumerable<string> input) {
        var labels = input.Single().Select(c => c - '0').ToArray();
        Simulate(labels, limit: 100)
            .StringJoin("")
            .Part1();

        var labels2 = labels.Concat(Enumerable.Range(labels.Max() + 1, 1_000_000 - labels.Length)).ToArray();
        Simulate(labels2, 10_000_000)
            .Take(2)
            .Product(x => (long)x)
            .Part2();
    }

    private class ListNode : IEnumerable<ListNode> {
        public ListNode(int label) => Label = label;
        public int Label { get; }
        public ListNode Next { get; set; } = null!;

        public IEnumerator<ListNode> GetEnumerator() => EnumerateNext().GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        private IEnumerable<ListNode> EnumerateNext() {
            var current = this;
            do {
                yield return current;
                current = current.Next;
            } while (current != this);
        }

        public override string ToString() => Label.ToString();
    }

    private static ListNode BuildList(IEnumerable<int> labels) {
        var dummyHead = new ListNode(-1);
        var current = dummyHead;
        foreach (var label in labels) {
            var newNode = new ListNode(label);
            current.Next = newNode;
            current = newNode;
        }

        current.Next = dummyHead.Next;
        return dummyHead.Next;
    }

    private static int[] Simulate(int[] labels, int limit) {
        var current = BuildList(labels);
        var ind = 0;
        var map = current.ToDictionary(x => x.Label);

        while (ind++ < limit) {
            var pickup1 = current.Next;
            var pickup2 = pickup1.Next;
            var pickup3 = pickup2.Next;
            var destination = GetDestination(current.Label);
            while (pickup1.Label == destination || pickup2.Label == destination || pickup3.Label == destination)
                destination = GetDestination(destination);

            var destNode = map[destination];
            (pickup3.Next, destNode.Next, current.Next) = (destNode.Next, pickup1, pickup3.Next);
            current = current.Next;
        }

        return map[1].Skip(1).Select(x => x.Label).ToArray();

        int GetDestination(int cur) => cur == 1 ? labels.Length : cur - 1;
    }
}