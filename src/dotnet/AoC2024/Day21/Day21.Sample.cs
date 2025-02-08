namespace AoC2024.Day21;

public static partial class Day21 {
    public static IEnumerable<SampleInput> GetSamples() {
        yield return SampleInput.ForInput("""
                                          029A
                                          980A
                                          179A
                                          456A
                                          379A
                                          """)
            .WithPartOneAnswer(126384);
    }
}
