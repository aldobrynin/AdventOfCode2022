namespace AoC2020.Day03;

public partial class Day03 {
    public static IEnumerable<SampleInput> GetSamples() {
        yield return SampleInput.ForInput("""
                                          ..##.......
                                          #...#...#..
                                          .#....#..#.
                                          ..#.#...#.#
                                          .#...##..#.
                                          ..#.##.....
                                          .#.#.#....#
                                          .#........#
                                          #.##...#...
                                          #...##....#
                                          .#..#...#.#
                                          """)
            .WithPartOneAnswer("7")
            .WithPartTwoAnswer("336");
    }
}