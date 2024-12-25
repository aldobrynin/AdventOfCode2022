namespace AoC2024.Day25;

public static partial class Day25 {
    public static IEnumerable<SampleInput> GetSamples() {
        yield return SampleInput.ForInput("""
                                          #####
                                          .####
                                          .####
                                          .####
                                          .#.#.
                                          .#...
                                          .....

                                          #####
                                          ##.##
                                          .#.##
                                          ...##
                                          ...#.
                                          ...#.
                                          .....

                                          .....
                                          #....
                                          #....
                                          #...#
                                          #.#.#
                                          #.###
                                          #####

                                          .....
                                          .....
                                          #.#..
                                          ###..
                                          ###.#
                                          ###.#
                                          #####

                                          .....
                                          .....
                                          .....
                                          #....
                                          #.#..
                                          #.#.#
                                          #####
                                          """)
            .WithPartOneAnswer(3);
    }
}
