namespace AoC2021.Day03;

public partial class Day03 {
    public static IEnumerable<SampleInput> GetSamples() {
        yield return SampleInput.ForInput("""
                                          00100
                                          11110
                                          10110
                                          10111
                                          10101
                                          01111
                                          00111
                                          11100
                                          10000
                                          11001
                                          00010
                                          01010
                                          """)
            .WithPartOneAnswer("198")
            .WithPartTwoAnswer("230");
    }
}