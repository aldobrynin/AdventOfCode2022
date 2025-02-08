namespace AoC2021.Day12;

public partial class Day12 {
    public static IEnumerable<SampleInput> GetSamples() {
        yield return SampleInput.ForInput("""
                                          fs-end
                                          he-DX
                                          fs-he
                                          start-DX
                                          pj-DX
                                          end-zg
                                          zg-sl
                                          zg-pj
                                          pj-he
                                          RW-he
                                          fs-DX
                                          pj-RW
                                          zg-RW
                                          start-pj
                                          he-WI
                                          zg-he
                                          pj-fs
                                          start-RW
                                          """)
            .WithPartOneAnswer("226")
            .WithPartTwoAnswer("3509");
    }
}