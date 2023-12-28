namespace AoC2019.Day05;

public static partial class Day05 {
    public static IEnumerable<SampleInput> GetSamples() {
        yield return SampleInput.ForInput("3,3,1105,-1,9,1101,0,0,12,4,12,99,1")
            .WithPartOneAnswer(1)
            .WithPartTwoAnswer(1);
    }
}