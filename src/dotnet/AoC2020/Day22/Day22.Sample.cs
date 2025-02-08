namespace AoC2020.Day22;

public partial class Day22 {
    public static IEnumerable<SampleInput> GetSamples() {
        yield return SampleInput.ForInput("""
                                          Player 1:
                                          9
                                          2
                                          6
                                          3
                                          1

                                          Player 2:
                                          5
                                          8
                                          4
                                          7
                                          10
                                          """)
            .WithPartOneAnswer("306")
            .WithPartTwoAnswer("291");
    }
}