namespace AoC2018.Day08;

public static partial class Day08 {
    public record TreeNode(int[] Metadata, TreeNode[] Children);

    public static void Solve(IEnumerable<string> input) {
        var data = input.SelectMany(x => x.ToIntArray()).ToArray();

        var (root, _) = Parse(0);
        SumMetadata(root).Part1();
        CalculateValue(root).Part2();

        (TreeNode, int) Parse(int index) {
            var childCount = data[index++];
            var metadataCount = data[index++];
            var children = new TreeNode[childCount];
            for (var i = 0; i < childCount; i++) {
                (children[i], index) = Parse(index);
            }

            var endOfNode = index + metadataCount;
            var metadata = data[index..endOfNode];
            return (new TreeNode(metadata, children), endOfNode);
        }

        int SumMetadata(TreeNode node) => node.Metadata.Sum() + node.Children.Sum(SumMetadata);

        int CalculateValue(TreeNode? node) {
            if (node is null) return 0;
            return node.Children.Length switch {
                0 => node.Metadata.Sum(),
                _ => node.Metadata.Sum(x => CalculateValue(node.Children.ElementAtOrDefault(x - 1)))
            };
        }
    }
}
