namespace AoC2018.Day08;

public static partial class Day08 {
    public static IEnumerable<SampleInput> GetSamples() {
        yield return SampleInput.ForInput("""
                                          2 3 0 3 10 11 12 1 1 0 1 99 2 1 1 2
                                          """)
            .WithPartOneAnswer(138)
            .WithPartTwoAnswer(66);
    }
}
