namespace AoC2021.Day25;

public partial class Day25 {
    public static IEnumerable<SampleInput> GetSamples() {
        yield return SampleInput.ForInput("""
                                          v...>>.vv>
                                          .vv>>.vv..
                                          >>.>v>...v
                                          >>v>>.>.v.
                                          v>v.vv.v..
                                          >.>>..v...
                                          .vv..>.>v.
                                          v.v..>>v.v
                                          ....v..v.>
                                          """)
            .WithPartOneAnswer("58");
    }
}