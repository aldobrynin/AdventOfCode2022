namespace AoC2023.Day09;

public partial class Day09 {
    public static IEnumerable<SampleInput> GetSamples() {
        yield return SampleInput.ForInput("""
                                          0 3 6 9 12 15
                                          1 3 6 10 15 21
                                          10 13 16 21 30 45
                                          """)
            .WithPartOneAnswer("114")
            .WithPartTwoAnswer("2");
    }
}