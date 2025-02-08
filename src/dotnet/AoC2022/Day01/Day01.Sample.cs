namespace AoC2022.Day01;

public partial class Day01 {
    public static IEnumerable<SampleInput> GetSamples() {
        yield return SampleInput.ForInput("""
                                          1000
                                          2000
                                          3000

                                          4000

                                          5000
                                          6000

                                          7000
                                          8000
                                          9000

                                          10000
                                          """)
            .WithPartOneAnswer("24000")
            .WithPartTwoAnswer("45000");
    }
}