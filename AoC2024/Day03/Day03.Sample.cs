namespace AoC2024.Day03;

public static partial class Day03 {
    public static IEnumerable<SampleInput> GetSamples() {
        yield return SampleInput
            .ForInput("xmul(2,4)&mul[3,7]!^don't()_mul(5,5)+mul(32,64](mul(11,8)undo()?mul(8,5))")
            .WithPartOneAnswer(161)
            .WithPartTwoAnswer(48);
    }
}
