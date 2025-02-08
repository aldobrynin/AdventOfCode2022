namespace AoC2022.Day12;

public partial class Day12 {
    public static IEnumerable<SampleInput> GetSamples() {
        yield return SampleInput.ForInput("""
                                          Sabqponm
                                          abcryxxl
                                          accszExk
                                          acctuvwj
                                          abdefghi
                                          """)
            .WithPartOneAnswer("31")
            .WithPartTwoAnswer("29");
    }
}