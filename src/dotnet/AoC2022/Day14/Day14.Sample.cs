namespace AoC2022.Day14;

public partial class Day14 {
    public static IEnumerable<SampleInput> GetSamples() {
        yield return SampleInput.ForInput("""
498,4 -> 498,6 -> 496,6
503,4 -> 502,4 -> 502,9 -> 494,9
""")
            .WithPartOneAnswer("24")
            .WithPartTwoAnswer("93");
    }
}