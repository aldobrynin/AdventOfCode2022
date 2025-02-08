namespace AoC2022.Day20;

public partial class Day20 {
    public static IEnumerable<SampleInput> GetSamples() {
        yield return SampleInput.ForInput("""
                                          1
                                          2
                                          -3
                                          3
                                          -2
                                          0
                                          4
                                          """)
            .WithPartOneAnswer("3")
            .WithPartTwoAnswer("1623178306");
    }
}