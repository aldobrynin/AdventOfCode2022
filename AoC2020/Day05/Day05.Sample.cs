namespace AoC2020.Day05;

public partial class Day05 {
    public static IEnumerable<SampleInput> GetSamples() {
        yield return SampleInput.ForInput("""
                                          FBFBBFFRLR
                                          BFFFBBFRRR
                                          FFFBBBFRRR
                                          BBFFBBFRLL
                                          """)
            .WithPartOneAnswer("820")
            .WithPartTwoAnswer("120");
    }
}