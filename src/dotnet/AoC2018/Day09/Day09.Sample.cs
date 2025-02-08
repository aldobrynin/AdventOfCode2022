namespace AoC2018.Day09;

public static partial class Day09 {
    public static IEnumerable<SampleInput> GetSamples() {
        yield return SampleInput.ForInput("""
                                          2 players; last marble is worth 25 points
                                          """)
            .WithPartOneAnswer(32);

        yield return SampleInput.ForInput("""
                                          10 players; last marble is worth 1618 points
                                          """)
            .WithPartOneAnswer(8317);
    }
}
