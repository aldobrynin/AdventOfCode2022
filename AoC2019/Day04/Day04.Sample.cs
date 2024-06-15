namespace AoC2019.Day04;

public static partial class Day04 {
    public static IEnumerable<SampleInput> GetSamples() {
        yield return SampleInput.ForInput("123444-123444").WithPartOneAnswer(1).WithPartTwoAnswer(0);
        yield return SampleInput.ForInput("111122-111122").WithPartOneAnswer(1).WithPartTwoAnswer(1);
    }
}