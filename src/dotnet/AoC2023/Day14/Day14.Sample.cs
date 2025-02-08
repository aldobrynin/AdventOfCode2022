namespace AoC2023.Day14;

public static partial class Day14 {
    public static IEnumerable<SampleInput> GetSamples() {
        yield return SampleInput.ForInput(
                """
                O....#....
                O.OO#....#
                .....##...
                OO.#O....O
                .O.....O#.
                O.#..O.#.#
                ..O..#O..O
                .......O..
                #....###..
                #OO..#....
                """)
            .WithPartOneAnswer(136).WithPartTwoAnswer(64);
    }
}