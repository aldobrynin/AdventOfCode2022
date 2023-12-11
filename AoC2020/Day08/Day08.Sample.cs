namespace AoC2020.Day08;

public partial class Day08 {
    public static IEnumerable<SampleInput> GetSamples() {
        yield return SampleInput.ForInput("""
                                          nop +0
                                          acc +1
                                          jmp +4
                                          acc +3
                                          jmp -3
                                          acc -99
                                          acc +1
                                          jmp -4
                                          acc +6
                                          """)
            .WithPartOneAnswer("5")
            .WithPartTwoAnswer("8");
    }
}