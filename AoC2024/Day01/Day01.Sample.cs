namespace AoC2024.Day01;

public static partial class Day01 {
    public static IEnumerable<SampleInput> GetSamples() {
        yield return SampleInput.ForInput("""
                                          3   4
                                          4   3
                                          2   5
                                          1   3
                                          3   9
                                          3   3
                                          """)
            .WithPartOneAnswer("11");

    }
}
