namespace AoC2018.Day02;

public static partial class Day02 {
    public static IEnumerable<SampleInput> GetSamples() {
        yield return SampleInput.ForInput("""
                                          abcdef
                                          bababc
                                          abbcde
                                          abcccd
                                          aabcdd
                                          abcdee
                                          ababab
                                          """)
            .WithPartOneAnswer(12);

        yield return SampleInput.ForInput("""
                                          abcde
                                          fghij
                                          klmno
                                          pqrst
                                          fguij
                                          axcye
                                          wvxyz
                                          """)
            .WithPartTwoAnswer("fgij");
    }
}
