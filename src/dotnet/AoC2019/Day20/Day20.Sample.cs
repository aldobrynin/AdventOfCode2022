namespace AoC2019.Day20;

public static partial class Day20 {
  public static IEnumerable<SampleInput> GetSamples() {
    yield return SampleInput.ForInput("""
                                               A         
                                               A         
                                        #######.#########
                                        #######.........#
                                        #######.#######.#
                                        #######.#######.#
                                        #######.#######.#
                                        #####  B    ###.#
                                      BC...##  C    ###.#
                                        ##.##       ###.#
                                        ##...DE  F  ###.#
                                        #####    G  ###.#
                                        #########.#####.#
                                      DE..#######...###.#
                                        #.#########.###.#
                                      FG..#########.....#
                                        ###########.#####
                                                   Z     
                                                   Z     
                                      """)
      .WithPartOneAnswer(23).WithPartTwoAnswer(26);
  }
}