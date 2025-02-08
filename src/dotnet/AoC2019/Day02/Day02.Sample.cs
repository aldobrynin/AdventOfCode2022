namespace AoC2019.Day02;

public static partial class Day02 {
    public static IEnumerable<SampleInput> GetSamples() {
        yield return SampleInput.ForInput("1,1,1,4,99,5,6,0,99")
            .WithPartOneAnswer("30,1,1,4,2,5,6,0,99");
    }
}