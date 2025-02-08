namespace AoC2021.Day09;

public partial class Day09 {
    public static IEnumerable<SampleInput> GetSamples() {
        yield return SampleInput.ForInput("""
                                          2199943210
                                          3987894921
                                          9856789892
                                          8767896789
                                          9899965678
                                          """)
            .WithPartOneAnswer("15")
            .WithPartTwoAnswer("1134");
    }
}