namespace AoC2020.Day01;

public partial class Day01 {
    public static IEnumerable<SampleInput> GetSamples() {
        yield return SampleInput.ForInput("""
                                          1721
                                          979
                                          366
                                          299
                                          675
                                          1456
                                          """)
            .WithPartOneAnswer("514579")
            .WithPartTwoAnswer("241861950");
    }
}