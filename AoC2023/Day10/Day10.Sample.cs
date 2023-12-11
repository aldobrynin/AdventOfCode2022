namespace AoC2023.Day10;

public partial class Day10 {
    public static IEnumerable<SampleInput> GetSamples() {
        yield return SampleInput.ForInput("""
                                          .F----7F7F7F7F-7....
                                          .|F--7||||||||FJ....
                                          .||.FJ||||||||L7....
                                          FJL7L7LJLJ||LJ.L-7..
                                          L--J.L7...LJS7F-7L7.
                                          ....F-J..F7FJ|L7L7L7
                                          ....L7.F7||L7|.L7L7|
                                          .....|FJLJ|FJ|F7|.LJ
                                          ....FJL-7.||.||||...
                                          ....L---J.LJ.LJLJ...
                                          """)
            .WithPartOneAnswer("70")
            .WithPartTwoAnswer("8");
    }
}