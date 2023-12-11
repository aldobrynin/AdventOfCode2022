namespace AoC2023.Day03;

public partial class Day03 {
    public static IEnumerable<SampleInput> GetSamples() {
        yield return SampleInput.ForInput("""
                                          467..114..
                                          ...*......
                                          ..35..633.
                                          ......#...
                                          617*......
                                          .....+.58.
                                          ..592.....
                                          ......755.
                                          ...$.*....
                                          .664.598..
                                          """)
            .WithPartOneAnswer("4361")
            .WithPartTwoAnswer("467835");
    }
}