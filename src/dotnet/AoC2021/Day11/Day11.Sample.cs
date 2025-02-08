namespace AoC2021.Day11;

public partial class Day11 {
    public static IEnumerable<SampleInput> GetSamples() {
        yield return SampleInput.ForInput("""
                                          5483143223
                                          2745854711
                                          5264556173
                                          6141336146
                                          6357385478
                                          4167524645
                                          2176841721
                                          6882881134
                                          4846848554
                                          5283751526
                                          """)
            .WithPartOneAnswer("1656")
            .WithPartTwoAnswer("195");
    }
}