namespace AoC2018.Day16;

public static partial class Day16 {

    public static IEnumerable<SampleInput> GetSamples() {
        yield return SampleInput.ForInput("""
                                          Before: [3, 2, 1, 1]
                                          9 2 1 2
                                          After:  [3, 2, 2, 1]
                                          """)
            .WithPartOneAnswer(1);
    }
}
