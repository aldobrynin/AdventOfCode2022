namespace AoC2018.Day03;

public static partial class Day03 {
    public record Claim(int Id, int X, int Y, int Width, int Height) {
        public Range2d<int> Range = Range2d.From(Common.Range.FromStartAndLength(X, Width),
            Common.Range.FromStartAndLength(Y, Height));

        public static Claim Parse(string input) {
            var parts = input.ToIntArray(" #@,:x");
            return new Claim(parts[0], parts[1], parts[2], parts[3], parts[4]);
        }
    }

    public static void Solve(IEnumerable<string> input) {
        var claims = input.Select(Claim.Parse).ToArray();

        var frequency = claims
            .SelectMany(x => x.Range.All())
            .CountFrequency();

        frequency
            .Count(x => x.Value > 1)
            .Part1();

        claims
            .Single(x => x.Range.All().All(v => frequency[v] == 1))
            .Id
            .Part2();
    }
}
