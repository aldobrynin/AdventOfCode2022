namespace AoC2018.Day06;

public static partial class Day06 {
    public static IEnumerable<SampleInput> GetSamples() {
        yield return SampleInput.ForInput("""
                                          1, 1
                                          1, 6
                                          8, 3
                                          3, 4
                                          5, 5
                                          8, 9
                                          """)
            .WithPartOneAnswer(17)
            .WithPartTwoAnswer(16);
    }
}
