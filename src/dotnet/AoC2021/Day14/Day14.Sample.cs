namespace AoC2021.Day14;

public partial class Day14 {
    public static IEnumerable<SampleInput> GetSamples() {
        yield return SampleInput.ForInput("""
                                          NNCB

                                          CH -> B
                                          HH -> N
                                          CB -> H
                                          NH -> C
                                          HB -> C
                                          HC -> B
                                          HN -> C
                                          NN -> C
                                          BH -> H
                                          NC -> B
                                          NB -> B
                                          BN -> B
                                          BB -> N
                                          BC -> B
                                          CC -> N
                                          CN -> C
                                          """)
            .WithPartOneAnswer("1588")
            .WithPartTwoAnswer("2188189693529");
    }
}