namespace AoC2023.Day06;

public partial class Day06 {
    public static IEnumerable<SampleInput> GetSamples() {
        yield return SampleInput.ForInput("""
                                          Time:      7  15   30
                                          Distance:  9  40  200
                                          """)
            .WithPartOneAnswer("288")
            .WithPartTwoAnswer("71503");
    }
}