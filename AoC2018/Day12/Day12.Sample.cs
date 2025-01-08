namespace AoC2018.Day12;

public static partial class Day12 {
    public static IEnumerable<SampleInput> GetSamples() {
        yield return SampleInput.ForInput("""
                                          initial state: #..#.#..##......###...###

                                          ...## => #
                                          ..#.. => #
                                          .#... => #
                                          .#.#. => #
                                          .#.## => #
                                          .##.. => #
                                          .#### => #
                                          #.#.# => #
                                          #.### => #
                                          ##.#. => #
                                          ##.## => #
                                          ###.. => #
                                          ###.# => #
                                          ####. => #
                                          """)
            .WithPartOneAnswer(325);
    }
}
