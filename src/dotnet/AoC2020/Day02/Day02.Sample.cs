namespace AoC2020.Day02;

public partial class Day02 {
    public static IEnumerable<SampleInput> GetSamples() {
        yield return SampleInput.ForInput("""
                                          1-3 a: abcde
                                          1-3 b: cdefg
                                          2-9 c: ccccccccc
                                          """)
            .WithPartOneAnswer("2")
            .WithPartTwoAnswer("1");
    }
}