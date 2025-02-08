namespace AoC2023.Day17;

public static partial class Day17 {
    public static IEnumerable<SampleInput> GetSamples() {
        yield return SampleInput.ForInput(
            """
            2413432311323
            3215453535623
            3255245654254
            3446585845452
            4546657867536
            1438598798454
            4457876987766
            3637877979653
            4654967986887
            4564679986453
            1224686865563
            2546548887735
            4322674655533
            """
        ).WithPartOneAnswer(102).WithPartTwoAnswer(94);

        yield return SampleInput.ForInput(
            """
            111111111111
            999999999991
            999999999991
            999999999991
            999999999991
            """
        ).WithPartTwoAnswer(71);
    }
}