namespace AoC2018.Day18;

public static partial class Day18 {
    public static IEnumerable<SampleInput> GetSamples() {
        yield return SampleInput.ForInput("""
                                          .#.#...|#.
                                          .....#|##|
                                          .|..|...#.
                                          ..|#.....#
                                          #.#|||#|#|
                                          ...#.||...
                                          .|....|...
                                          ||...#|.#|
                                          |.||||..|.
                                          ...#.|..|.
                                          """)
            .WithPartOneAnswer(1147);
    }
}
