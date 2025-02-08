namespace AoC2021.Day17;

public partial class Day17 {
    public static IEnumerable<SampleInput> GetSamples() {
        yield return SampleInput.ForInput("target area: x=20..30, y=-10..-5")
            .WithPartOneAnswer("45")
            .WithPartTwoAnswer("112");
    }
}