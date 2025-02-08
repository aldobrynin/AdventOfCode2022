namespace AoC2024.Day25;

public static partial class Day25 {
    public record Schema(Map<char> Map) {
        public readonly bool IsKey = Map.TopBorder().All(v => Map[v] == '.');
        public readonly int[] Heights = Map.Columns().Select(col => col.Count(x => x == '#') - 1).ToArray();

        public static Schema From(Map<char> map) => new Schema(map);

        public bool IsMatch(Schema other) {
            return IsKey ^ other.IsKey &&
                   Heights.Zip(other.Heights, (a, b) => a + b).All(x => x <= Map.SizeX);
        }
    }

    public static void Solve(IEnumerable<string> input) {
        input.SplitBy(string.IsNullOrEmpty)
            .Select(Map.From)
            .Select(Schema.From)
            .Pairs()
            .Count(x => x.A.IsMatch(x.B))
            .Part1();
    }
}
