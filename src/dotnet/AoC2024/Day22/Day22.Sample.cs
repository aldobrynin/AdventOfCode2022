namespace AoC2024.Day22;

public static partial class Day22 {
    public static IEnumerable<SampleInput> GetSamples() {
        yield return SampleInput.ForInput("""
                                          1
                                          10
                                          100
                                          2024
                                          """)
            .WithPartOneAnswer(37327623)
            .WithPartTwoAnswer(24);

        yield return SampleInput.ForInput("""
                                          1
                                          2
                                          3
                                          2024
                                          """)
            .WithPartOneAnswer(37990510)
            .WithPartTwoAnswer(23);
    }
}
