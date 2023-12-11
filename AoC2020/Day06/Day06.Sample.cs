namespace AoC2020.Day06;

public partial class Day06 {
    public static IEnumerable<SampleInput> GetSamples() {
        yield return SampleInput.ForInput("""
                                          abc

                                          a
                                          b
                                          c

                                          ab
                                          ac

                                          a
                                          a
                                          a
                                          a

                                          b
                                          """)
            .WithPartOneAnswer("11")
            .WithPartTwoAnswer("6");
    }
}