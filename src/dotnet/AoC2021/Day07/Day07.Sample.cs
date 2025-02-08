namespace AoC2021.Day07;

public partial class Day07 {
    public static IEnumerable<SampleInput> GetSamples() {
        yield return SampleInput.ForInput("16,1,2,0,4,2,7,1,2,14")
            .WithPartOneAnswer("37")
            .WithPartTwoAnswer("168");
    }
}