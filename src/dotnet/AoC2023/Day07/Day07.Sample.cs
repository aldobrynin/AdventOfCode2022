namespace AoC2023.Day07;

public partial class Day07 {
    public static IEnumerable<SampleInput> GetSamples() {
        yield return SampleInput.ForInput("""
                                          32T3K 765
                                          T55J5 684
                                          KK677 28
                                          KTJJT 220
                                          QQQJA 483
                                          """)
            .WithPartOneAnswer("6440")
            .WithPartTwoAnswer("5905");
    }
}