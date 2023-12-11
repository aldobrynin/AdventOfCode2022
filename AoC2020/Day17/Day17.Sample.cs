namespace AoC2020.Day17;

public partial class Day17 {
    public static IEnumerable<SampleInput> GetSamples() {
        yield return SampleInput.ForInput("""
                                          .#.
                                          ..#
                                          ###
                                          """)
            .WithPartOneAnswer("112")
            .WithPartTwoAnswer("848");
    }
}