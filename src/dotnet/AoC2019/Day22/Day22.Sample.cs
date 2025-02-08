namespace AoC2019.Day22;

public static partial class Day22 {
    public static IEnumerable<SampleInput> GetSamples() {
        yield return SampleInput.ForInput("""
                                          cut 6
                                          deal with increment 7
                                          deal into new stack
                                          """)
            // not really sure how to test this one
            .WithPartOneAnswer(5922)
            .WithPartTwoAnswer(118925585929508);
    }
}