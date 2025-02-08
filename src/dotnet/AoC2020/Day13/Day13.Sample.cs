namespace AoC2020.Day13;

public partial class Day13 {
    public static IEnumerable<SampleInput> GetSamples() {
        yield return SampleInput.ForInput("""
                                          939
                                          7,13,x,x,59,x,31,19
                                          """)
            .WithPartOneAnswer("295")
            .WithPartTwoAnswer("1068781");
    }
}