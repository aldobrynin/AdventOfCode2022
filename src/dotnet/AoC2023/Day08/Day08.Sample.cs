namespace AoC2023.Day08;

public partial class Day08 {
    public static IEnumerable<SampleInput> GetSamples() {
        yield return SampleInput.ForInput("""
                                          RL

                                          AAA = (BBB, CCC)
                                          BBB = (DDD, EEE)
                                          CCC = (ZZZ, GGG)
                                          DDD = (DDD, DDD)
                                          EEE = (EEE, EEE)
                                          GGG = (GGG, GGG)
                                          ZZZ = (ZZZ, ZZZ)
                                          """)
            .WithPartOneAnswer("2")
            .WithPartTwoAnswer("2");
    }
}