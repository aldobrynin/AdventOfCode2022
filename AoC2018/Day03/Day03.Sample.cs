namespace AoC2018.Day03;

public static partial class Day03 {
    public static IEnumerable<SampleInput> GetSamples() {
        yield return SampleInput.ForInput("""
                                          #1 @ 1,3: 4x4
                                          #2 @ 3,1: 4x4
                                          #3 @ 5,5: 2x2
                                          """)
            .WithPartOneAnswer(4)
            .WithPartTwoAnswer(3);
    }
}
