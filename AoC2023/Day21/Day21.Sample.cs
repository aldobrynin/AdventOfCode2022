namespace AoC2023.Day21;

public static partial class Day21 {
    public static IEnumerable<SampleInput> GetSamples() {
        yield return SampleInput.ForInput(
            """
            ...........
            .....###.#.
            .###.##..#.
            ..#.#...#..
            ....#.#....
            .##..S####.
            .##..#...#.
            .......##..
            .##.#.####.
            .##..##.##.
            ...........
            """
        ).WithPartOneAnswer(16)
        .WithPartTwoAnswer(16733044);
    }
}