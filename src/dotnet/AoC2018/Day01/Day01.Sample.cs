namespace AoC2018.Day01;

public static partial class Day01 {
    public static IEnumerable<SampleInput> GetSamples() {
        yield return SampleInput.ForInput("""
                                          +1, -2, +3, +1
                                          """)
            .WithPartOneAnswer(3)
            .WithPartTwoAnswer(2);
    }
}
