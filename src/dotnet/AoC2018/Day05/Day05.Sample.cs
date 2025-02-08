namespace AoC2018.Day05;

public static partial class Day05 {
    public static IEnumerable<SampleInput> GetSamples() {
        yield return SampleInput.ForInput("""
                                          dabAcCaCBAcCcaDA
                                          """)
            .WithPartOneAnswer(10)
            .WithPartTwoAnswer(4);
    }
}
