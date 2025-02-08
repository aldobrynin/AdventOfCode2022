namespace AoC2020.Day15;

public partial class Day15 {
    public static IEnumerable<SampleInput> GetSamples() {
        yield return SampleInput.ForInput("0,3,6")
            .WithPartOneAnswer("436")
            .WithPartTwoAnswer("175594");
    }
}