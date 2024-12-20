namespace AoC2024.Day20;

public static partial class Day20 {
    public static IEnumerable<SampleInput> GetSamples() {
        yield return SampleInput.ForInput("""
                                          ###############
                                          #...#...#.....#
                                          #.#.#.#.#.###.#
                                          #S#...#.#.#...#
                                          #######.#.#.###
                                          #######.#.#...#
                                          #######.#.###.#
                                          ###..E#...#...#
                                          ###.#######.###
                                          #...###...#...#
                                          #.#####.#.###.#
                                          #.#...#.#.#...#
                                          #.#.#.#.#.#.###
                                          #...#...#...###
                                          ###############
                                          """)
            .WithPartOneAnswer(1)
            .WithPartTwoAnswer(86);
    }
}
