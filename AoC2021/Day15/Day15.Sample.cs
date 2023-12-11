namespace AoC2021.Day15;

public partial class Day15 {
    public static IEnumerable<SampleInput> GetSamples() {
        yield return SampleInput.ForInput("""
                                          1163751742
                                          1381373672
                                          2136511328
                                          3694931569
                                          7463417111
                                          1319128137
                                          1359912421
                                          3125421639
                                          1293138521
                                          2311944581
                                          """)
            .WithPartOneAnswer("40")
            .WithPartTwoAnswer("315");
    }
}