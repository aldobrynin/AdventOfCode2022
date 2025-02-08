namespace AoC2023.Day12;

public partial class Day12 {
    public static IEnumerable<SampleInput> GetSamples() {
        yield return SampleInput.ForInput("""
                                          ???.### 1,1,3
                                          .??..??...?##. 1,1,3
                                          ?#?#?#?#?#?#?#? 1,3,1,6
                                          ????.#...#... 4,1,1
                                          ????.######..#####. 1,6,5
                                          ?###???????? 3,2,1
                                          """)
            .WithPartOneAnswer(21)
            .WithPartTwoAnswer(525152);
    }
}