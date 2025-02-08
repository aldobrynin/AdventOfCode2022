namespace AoC2020.Day09;

public partial class Day09 {
    public static IEnumerable<SampleInput> GetSamples() {
        yield return SampleInput.ForInput("""
                                          35
                                          20
                                          15
                                          25
                                          47
                                          40
                                          62
                                          55
                                          65
                                          95
                                          102
                                          117
                                          150
                                          182
                                          127
                                          219
                                          299
                                          277
                                          309
                                          576
                                          """)
            .WithPartOneAnswer("127")
            .WithPartTwoAnswer("62");
    }
}