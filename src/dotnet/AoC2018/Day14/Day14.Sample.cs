namespace AoC2018.Day14;

public static partial class Day14 {
    public static IEnumerable<SampleInput> GetSamples() {
        yield return SampleInput.ForInput("9").WithPartOneAnswer("5158916779");
        yield return SampleInput.ForInput("5").WithPartOneAnswer("0124515891");
        yield return SampleInput.ForInput("18").WithPartOneAnswer("9251071085");
        yield return SampleInput.ForInput("2018").WithPartOneAnswer("5941429882");
        yield return SampleInput.ForInput("59414").WithPartTwoAnswer("2018");
    }
}
