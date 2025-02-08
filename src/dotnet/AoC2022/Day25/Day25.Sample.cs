namespace AoC2022.Day25;

public partial class Day25 {
    public static IEnumerable<SampleInput> GetSamples() {
        yield return SampleInput.ForInput("""
                                          1=-0-2
                                          12111
                                          2=0=
                                          21
                                          2=01
                                          111
                                          20012
                                          112
                                          1=-1=
                                          1-12
                                          12
                                          1=
                                          122
                                          """)
            .WithPartOneAnswer("2=-1=0");
    }
}