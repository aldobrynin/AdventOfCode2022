namespace AoC2020.Day18;

public partial class Day18 {
    public static IEnumerable<SampleInput> GetSamples() {
        yield return SampleInput.ForInput("""
                                          1 + 2 * 3 + 4 * 5 + 6
                                          1 + (2 * 3) + (4 * (5 + 6))
                                          2 * 3 + (4 * 5)
                                          5 + (8 * 3 + 9 + 3 * 4 * 3)
                                          5 * 9 * (7 * 3 * 3 + 9 * 3 + (8 + 6 * 4))
                                          ((2 + 4 * 9) * (6 + 9 * 8 + 6) + 6) + 2 + 4 * 2
                                          """)
            .WithPartOneAnswer("26457")
            .WithPartTwoAnswer("694173");
    }
}