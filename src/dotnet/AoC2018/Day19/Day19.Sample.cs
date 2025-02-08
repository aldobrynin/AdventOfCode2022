namespace AoC2018.Day19;

public static partial class Day19 {
    public static IEnumerable<SampleInput> GetSamples() {
        yield return SampleInput.ForInput("""
                                          #ip 0
                                          seti 5 0 1
                                          seti 6 0 2
                                          addi 0 1 0
                                          addr 1 2 3
                                          setr 1 0 0
                                          seti 8 0 4
                                          seti 9 0 5
                                          """);
    }
}
