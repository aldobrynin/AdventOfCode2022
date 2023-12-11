namespace AoC2021.Day06;

public partial class Day06 {
    public static IEnumerable<SampleInput> GetSamples() {
        yield return SampleInput.ForInput("3,4,3,1,2")
            .WithPartOneAnswer("5934")
            .WithPartTwoAnswer("26984457539");
    }
}