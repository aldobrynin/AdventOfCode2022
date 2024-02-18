namespace AoC2019.Day24;

public static partial class Day24 {
    public static IEnumerable<SampleInput> GetSamples() {
        yield return SampleInput.ForInput("""
                                          ....#
                                          #..#.
                                          #..##
                                          ..#..
                                          #....
                                          """)
            .WithPartOneAnswer(2129920);
    }
}