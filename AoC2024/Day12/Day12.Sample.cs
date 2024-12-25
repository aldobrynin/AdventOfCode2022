namespace AoC2024.Day12;

public static partial class Day12 {
    public static IEnumerable<SampleInput> GetSamples() {
        yield return SampleInput.ForInput("""
                                          AAAA
                                          BBCD
                                          BBCC
                                          EEEC
                                          """)
            .WithPartOneAnswer(140)
            .WithPartTwoAnswer(80);

        yield return SampleInput.ForInput("""
                                          RRRRIICCFF
                                          RRRRIICCCF
                                          VVRRRCCFFF
                                          VVRCCCJFFF
                                          VVVVCJJCFE
                                          VVIVCCJJEE
                                          VVIIICJJEE
                                          MIIIIIJJEE
                                          MIIISIJEEE
                                          MMMISSJEEE
                                          """)
            .WithPartOneAnswer(1930)
            .WithPartTwoAnswer(1206);
    }
}
