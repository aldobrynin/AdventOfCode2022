namespace AoC2024.Day19;

public static partial class Day19 {
    public static IEnumerable<SampleInput> GetSamples() {
        yield return SampleInput.ForInput("""
                                          r, wr, b, g, bwu, rb, gb, br

                                          brwrr
                                          bggr
                                          gbbr
                                          rrbgbr
                                          ubwu
                                          bwurrg
                                          brgr
                                          bbrgwb
                                          """)
            .WithPartOneAnswer(6)
            .WithPartTwoAnswer(16);
    }
}
