namespace AoC2021.Day01;

public partial class Day01 {
    public static IEnumerable<SampleInput> GetSamples() {
        yield return SampleInput.ForInput("""
                                          199
                                          200
                                          208
                                          210
                                          200
                                          207
                                          240
                                          269
                                          260
                                          263
                                          """)
            .WithPartOneAnswer("7")
            .WithPartTwoAnswer("5");
    }
}