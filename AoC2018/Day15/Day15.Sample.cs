namespace AoC2018.Day15;

public static partial class Day15 {
    public static IEnumerable<SampleInput> GetSamples() {
        yield return SampleInput.ForInput("""
                                          #######
                                          #.G...#
                                          #...EG#
                                          #.#.#G#
                                          #..G#E#
                                          #.....#
                                          #######
                                          """)
            .WithPartOneAnswer(27730)
            .WithPartTwoAnswer(4988);

        yield return SampleInput.ForInput("""
                                          #######
                                          #G..#E#
                                          #E#E.E#
                                          #G.##.#
                                          #...#E#
                                          #...E.#
                                          #######
                                          """)
            .WithPartOneAnswer(36334);

        yield return SampleInput.ForInput("""
                                          #######
                                          #E..EG#
                                          #.#G.E#
                                          #E.##E#
                                          #G..#.#
                                          #..E#.#
                                          #######
                                          """)
            .WithPartOneAnswer(39514)
            .WithPartTwoAnswer(31284);

        yield return SampleInput.ForInput("""
                                          #######
                                          #E.G#.#
                                          #.#G..#
                                          #G.#.G#
                                          #G..#.#
                                          #...E.#
                                          #######
                                          """)
            .WithPartOneAnswer(27755)
            .WithPartTwoAnswer(3478);

        yield return SampleInput.ForInput("""
                                          #######
                                          #.E...#
                                          #.#..G#
                                          #.###.#
                                          #E#G#G#
                                          #...#G#
                                          #######
                                          """)
            .WithPartOneAnswer(28944)
            .WithPartTwoAnswer(6474);

        yield return SampleInput.ForInput("""
                                          #########
                                          #G......#
                                          #.E.#...#
                                          #..##..G#
                                          #...##..#
                                          #...#...#
                                          #.G...G.#
                                          #.....G.#
                                          #########
                                          """)
            .WithPartOneAnswer(18740)
            .WithPartTwoAnswer(1140);
    }
}
