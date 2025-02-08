namespace AoC2021.Day23;

public partial class Day23 {
    public static IEnumerable<SampleInput> GetSamples() {
        yield return SampleInput.ForInput("""
                                          #############
                                          #...........#
                                          ###B#C#B#D###
                                            #A#D#C#A#
                                            #########
                                          """)
            .WithPartOneAnswer("12521")
            .WithPartTwoAnswer("44169");
    }
}