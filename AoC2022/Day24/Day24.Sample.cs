namespace AoC2022.Day24;

public partial class Day24 {
    public static IEnumerable<SampleInput> GetSamples() {
        yield return SampleInput.ForInput("""
                                          #.######
                                          #>>.<^<#
                                          #.<..<<#
                                          #>v.><>#
                                          #<^v^^>#
                                          ######.#
                                          """)
            .WithPartOneAnswer("18")
            .WithPartTwoAnswer("54");
    }
}