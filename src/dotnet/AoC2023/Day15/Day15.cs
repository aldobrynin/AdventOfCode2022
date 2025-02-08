namespace AoC2023.Day15;

public static partial class Day15 {
    public static void Solve(IEnumerable<string> input) {
        var lines = input.Single().Split(',').ToArray();

        lines.Sum(CalculateHash).Part1();

        BuildHashMap(lines).WithIndex()
            .Sum(box => box.Element.Indices()
                .Sum(slot => (box.Index + 1L) * (slot + 1) * box.Element[slot].FocalLength))
            .Part2();
    }

    private static List<(string Label, int FocalLength)>[] BuildHashMap(string[] lines) {
        var boxes = new List<(string Label, int FocalLength)>[256];
        for (var i = 0; i < 256; i++) boxes[i] = [];

        foreach (var line in lines) {
            var parts = line.Split(new[] { '=', '-' }, StringSplitOptions.RemoveEmptyEntries);
            var label = parts[0];
            var box = boxes[CalculateHash(label)];
            if (parts.Length == 1) {
                var boxIndex = box.FindIndex(x => x.Label == label);
                if (boxIndex != -1) box.RemoveAt(boxIndex);
            }
            else {
                var boxIndex = box.FindIndex(x => x.Label == label);
                var focalLength = parts[1].ToInt();
                if (boxIndex == -1) box.Add((label, focalLength));
                else box[boxIndex] = (label, focalLength);
            }
        }

        return boxes;
    }

    private static int CalculateHash(string s) => s.Aggregate(0, (acc, cur) => (acc + cur) * 17 % 256);
}