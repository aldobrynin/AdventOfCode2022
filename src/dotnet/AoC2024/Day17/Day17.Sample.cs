namespace AoC2024.Day17;

public static partial class Day17 {
    public static IEnumerable<SampleInput> GetSamples() {
        yield return SampleInput.ForInput("""
                                          Register A: 729
                                          Register B: 0
                                          Register C: 0

                                          Program: 0,1,5,4,3,0
                                          """)
            .WithPartOneAnswer("4,6,3,5,6,3,5,2,1,0");

        yield return SampleInput.ForInput("""
                                          Register A: 2024
                                          Register B: 0
                                          Register C: 0

                                          Program: 0,3,5,4,3,0
                                          """)
            .WithPartOneAnswer("5,7,3,0")
            .WithPartTwoAnswer(117440);
    }
}
