namespace AoC2024.Day02;

public static partial class Day02 {
    public static IEnumerable<SampleInput> GetSamples() {
        yield return SampleInput.ForInput("""
                                          7 6 4 2 1
                                          1 2 7 8 9
                                          9 7 6 2 1
                                          1 3 2 4 5
                                          8 6 4 4 1
                                          1 3 6 7 9
                                          """)
            .WithPartOneAnswer(2)
            .WithPartTwoAnswer(4);
    }
}
