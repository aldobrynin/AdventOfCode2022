namespace AoC2022.Day17;

public partial class Day17 {
    public static IEnumerable<SampleInput> GetSamples() {
        yield return SampleInput.ForInput(">>><<><>><<<>><>>><<<>>><<<><<<>><>><<>>")
            .WithPartOneAnswer("3068")
            .WithPartTwoAnswer("1514285714288");
    }
}