namespace AoC2018.Day13;

public static partial class Day13 {
    public static IEnumerable<SampleInput> GetSamples() {
        yield return SampleInput.ForInput("""
                                          /->-\........
                                          |   |  /----\
                                          | /-+--+-\  |
                                          | | |  | v  |
                                          \-+-/  \-+--/
                                            \------/...
                                          """)
            .WithPartOneAnswer("7,3");

        yield return SampleInput.ForInput("""
                                          />-<\..
                                          |   |..
                                          | /<+-\
                                          | | | v
                                          \>+</ |
                                            |   ^
                                            \<->/
                                          """)
            .WithPartTwoAnswer("6,4");
    }
}
