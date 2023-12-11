namespace AoC2022.Day04;

public partial class Day04 {
    public static IEnumerable<SampleInput> GetSamples() {
        yield return SampleInput.ForInput("""
                                          2-4,6-8
                                          2-3,4-5
                                          5-7,7-9
                                          2-8,3-7
                                          6-6,4-6
                                          2-6,4-8
                                          """)
            .WithPartOneAnswer("2")
            .WithPartTwoAnswer("4");
    }
}