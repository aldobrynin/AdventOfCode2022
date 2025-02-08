namespace AoC2020.Day12;

public partial class Day12 {
    public static IEnumerable<SampleInput> GetSamples() {
        yield return SampleInput.ForInput("""
                                          F10
                                          N3
                                          F7
                                          R90
                                          F11
                                          """)
            .WithPartOneAnswer("25")
            .WithPartTwoAnswer("286");
    }
}