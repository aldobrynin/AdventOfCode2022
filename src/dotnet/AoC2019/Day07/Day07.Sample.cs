namespace AoC2019.Day07;

public static partial class Day07 {
    public static IEnumerable<SampleInput> GetSamples() {
        yield return SampleInput.ForInput("3,15,3,16,1002,16,10,16,1,16,15,15,4,15,99,0,0")
            .WithPartOneAnswer(43210)
            .WithPartTwoAnswer(98765);
    }
}