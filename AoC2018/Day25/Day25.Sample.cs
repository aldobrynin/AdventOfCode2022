namespace AoC2018.Day25;

public static partial class Day25 {
    public static IEnumerable<SampleInput> GetSamples() {
        yield return SampleInput.ForInput("""
                                           0,0,0,0
                                           3,0,0,0
                                           0,3,0,0
                                           0,0,3,0
                                           0,0,0,3
                                           0,0,0,6
                                           9,0,0,0
                                          12,0,0,0
                                          """)
            .WithPartOneAnswer(2);

        yield return SampleInput.ForInput("""
                                          -1,2,2,0
                                          0,0,2,-2
                                          0,0,0,-2
                                          -1,2,0,0
                                          -2,-2,-2,2
                                          3,0,2,-1
                                          -1,3,2,2
                                          -1,0,-1,0
                                          0,2,1,-2
                                          3,0,0,0
                                          """)
            .WithPartOneAnswer(4);

        yield return SampleInput.ForInput("""
                                          1,-1,-1,-2
                                          -2,-2,0,1
                                          0,2,1,3
                                          -2,3,-2,1
                                          0,2,3,-2
                                          -1,-1,1,-2
                                          0,-2,-1,0
                                          -2,2,3,-1
                                          1,2,2,0
                                          -1,-2,0,-2
                                          """)
            .WithPartOneAnswer(8);
    }
}
