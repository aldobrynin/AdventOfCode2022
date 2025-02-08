namespace AoC2022.Day05;

public partial class Day05 {
    public static IEnumerable<SampleInput> GetSamples() {
        yield return SampleInput.ForInput("""
                                              [D]    
                                          [N] [C]    
                                          [Z] [M] [P]
                                           1   2   3 
                                          
                                          move 1 from 2 to 1
                                          move 3 from 1 to 3
                                          move 2 from 2 to 1
                                          move 1 from 1 to 2
                                          """)
            .WithPartOneAnswer("CMZ")
            .WithPartTwoAnswer("MCD");
    }
}