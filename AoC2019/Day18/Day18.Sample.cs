namespace AoC2019.Day18;

public static partial class Day18 {
    public static IEnumerable<SampleInput> GetSamples() {
        yield return SampleInput.ForInput("""
                                          ###############
                                          #d.ABC.#.....a#
                                          ######...######
                                          ######.@.######
                                          ######...######
                                          #b.....#.....c#
                                          ###############
                                          """)
            .WithPartOneAnswer(52)
            .WithPartTwoAnswer(24);
    }
}