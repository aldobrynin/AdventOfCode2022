namespace AoC2022.Day18;

public partial class Day18 {
    public static IEnumerable<SampleInput> GetSamples() {
        yield return SampleInput.ForInput("""
                                          2,2,2
                                          1,2,2
                                          3,2,2
                                          2,1,2
                                          2,3,2
                                          2,2,1
                                          2,2,3
                                          2,2,4
                                          2,2,6
                                          1,2,5
                                          3,2,5
                                          2,1,5
                                          2,3,5
                                          """)
            .WithPartOneAnswer("64")
            .WithPartTwoAnswer("58");
    }
}