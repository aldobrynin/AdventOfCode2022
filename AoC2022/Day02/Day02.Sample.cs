namespace AoC2022.Day02;

public partial class Day02 {
    public static IEnumerable<SampleInput> GetSamples() {
        yield return SampleInput.ForInput("""
                                          A Y
                                          B X
                                          C Z
                                          """)
            .WithPartOneAnswer("15")
            .WithPartTwoAnswer("12");
    }
}