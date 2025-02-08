namespace AoC2022.Day22;

public partial class Day22 {
        public static IEnumerable<SampleInput> GetSamples() {
                yield return SampleInput.ForInput("""
                                                          ...#
                                                          .#..
                                                          #...
                                                          ....
                                                  ...#.......#
                                                  ........#...
                                                  ..#....#....
                                                  ..........#.
                                                          ...#....
                                                          .....#..
                                                          .#......
                                                          ......#.

                                                  10R5L5R10L4R5L5
                                                  """)
                        .WithPartOneAnswer("6032")
                        .WithPartTwoAnswer("5031");
        }
}