namespace AoC2020.Day11;

public partial class Day11 {
    public static IEnumerable<SampleInput> GetSamples() {
        yield return SampleInput.ForInput("""
                                          L.LL.LL.LL
                                          LLLLLLL.LL
                                          L.L.L..L..
                                          LLLL.LL.LL
                                          L.LL.LL.LL
                                          L.LLLLL.LL
                                          ..L.L.....
                                          LLLLLLLLLL
                                          L.LLLLLL.L
                                          L.LLLLL.LL
                                          """)
            .WithPartOneAnswer("37")
            .WithPartTwoAnswer("26");
    }
}