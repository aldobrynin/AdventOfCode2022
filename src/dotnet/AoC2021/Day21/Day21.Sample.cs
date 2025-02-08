namespace AoC2021.Day21;

public partial class Day21 {
    public static IEnumerable<SampleInput> GetSamples() {
        yield return SampleInput.ForInput("""
                                          Player 1 starting position: 4
                                          Player 2 starting position: 8
                                          """)
            .WithPartOneAnswer("739785")
            .WithPartTwoAnswer("444356092776315");
    }
}