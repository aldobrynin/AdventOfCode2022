namespace AoC2024.Day04;

public static partial class Day04 {
    public static IEnumerable<SampleInput> GetSamples() {
        yield return SampleInput.ForInput("""
                                          MMMSXXMASM
                                          MSAMXMSMSA
                                          AMXSXMAAMM
                                          MSAMASMSMX
                                          XMASAMXAMM
                                          XXAMMXXAMA
                                          SMSMSASXSS
                                          SAXAMASAAA
                                          MAMMMXMMMM
                                          MXMXAXMASX
                                          """)
            .WithPartOneAnswer(18)
            .WithPartTwoAnswer(9);
    }
}
