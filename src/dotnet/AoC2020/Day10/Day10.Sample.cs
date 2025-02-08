namespace AoC2020.Day10;

public partial class Day10 {
    public static IEnumerable<SampleInput> GetSamples() {
        yield return SampleInput.ForInput("""
                                          28
                                          33
                                          18
                                          42
                                          31
                                          14
                                          46
                                          20
                                          48
                                          47
                                          24
                                          23
                                          49
                                          45
                                          19
                                          38
                                          39
                                          11
                                          1
                                          32
                                          25
                                          35
                                          8
                                          17
                                          7
                                          9
                                          4
                                          2
                                          34
                                          10
                                          3
                                          """)
            .WithPartOneAnswer("220")
            .WithPartTwoAnswer("19208");
    }
}