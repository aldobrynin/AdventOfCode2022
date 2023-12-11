namespace AoC2021.Day02;

public partial class Day02 {
    public static IEnumerable<SampleInput> GetSamples() {
        yield return SampleInput.ForInput("""
                                          forward 5
                                          down 5
                                          forward 8
                                          up 3
                                          down 8
                                          forward 2
                                          """)
            .WithPartOneAnswer("150")
            .WithPartTwoAnswer("900");
    }
}