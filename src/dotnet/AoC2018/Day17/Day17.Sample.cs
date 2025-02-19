namespace AoC2018.Day17;

public static partial class Day17 {
    public static IEnumerable<SampleInput> GetSamples() {
        yield return SampleInput.ForInput("""
                                          x=495, y=2..7
                                          y=7, x=495..501
                                          x=501, y=3..7
                                          x=498, y=2..4
                                          x=506, y=1..2
                                          x=498, y=10..13
                                          x=504, y=10..13
                                          y=13, x=498..504
                                          """)
            .WithPartOneAnswer(57)
            .WithPartTwoAnswer(29);
    }
}
