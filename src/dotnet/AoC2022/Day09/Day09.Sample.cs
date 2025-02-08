namespace AoC2022.Day09;

public partial class Day09 {
    public static IEnumerable<SampleInput> GetSamples() {
        yield return SampleInput.ForInput("""
                                          R 4
                                          U 4
                                          L 3
                                          D 1
                                          R 4
                                          D 1
                                          L 5
                                          R 2
                                          """)
            .WithPartOneAnswer("13")
            .WithPartTwoAnswer("1");
    }
}