namespace AoC2020.Day14;

public partial class Day14 {
    public static IEnumerable<SampleInput> GetSamples() {
        yield return SampleInput.ForInput("""
                                          mask = 000000000000000000000000000000X1001X
                                          mem[42] = 100
                                          mask = 00000000000000000000000000000000X0XX
                                          mem[26] = 1
                                          """)
            .WithPartOneAnswer("51")
            .WithPartTwoAnswer("208");
    }
}