    namespace AoC2023.Day13;

    public partial class Day13 {
        public static IEnumerable<SampleInput> GetSamples() {
            yield return SampleInput.ForInput("""
                                              #.##..##.
                                              ..#.##.#.
                                              ##......#
                                              ##......#
                                              ..#.##.#.
                                              ..##..##.
                                              #.#.##.#.

                                              #...##..#
                                              #....#..#
                                              ..##..###
                                              #####.##.
                                              #####.##.
                                              ..##..###
                                              #....#..#
                                              """
            ).WithPartOneAnswer("405")
            .WithPartTwoAnswer("400");
        }
    }