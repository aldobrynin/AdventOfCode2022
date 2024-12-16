namespace AoC2024.Day16;

public static partial class Day16 {
    public static IEnumerable<SampleInput> GetSamples() {
        yield return SampleInput.ForInput("""
                                          ###############
                                          #.......#....E#
                                          #.#.###.#.###.#
                                          #.....#.#...#.#
                                          #.###.#####.#.#
                                          #.#.#.......#.#
                                          #.#.#####.###.#
                                          #...........#.#
                                          ###.#.#####.#.#
                                          #...#.....#.#.#
                                          #.#.#.###.#.#.#
                                          #.....#...#.#.#
                                          #.###.#.#.#.#.#
                                          #S..#.....#...#
                                          ###############
                                          """)
            .WithPartOneAnswer(7036)
            .WithPartTwoAnswer(45);
    }
}
