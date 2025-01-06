namespace AoC2018.Day22;

public static partial class Day22 {
    public static IEnumerable<SampleInput> GetSamples() {
        yield return SampleInput.ForInput("""
                                          depth: 510
                                          target: 10,10
                                          """)
            .WithPartOneAnswer(114)
            .WithPartTwoAnswer(45);
    }
}
