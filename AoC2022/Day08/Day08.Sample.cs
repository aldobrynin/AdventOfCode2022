namespace AoC2022.Day08;

public partial class Day08 {
    public static IEnumerable<SampleInput> GetSamples() {
        yield return SampleInput.ForInput("""
                                          30373
                                          25512
                                          65332
                                          33549
                                          35390
                                          """)
            .WithPartOneAnswer("21")
            .WithPartTwoAnswer("8");
    }
}