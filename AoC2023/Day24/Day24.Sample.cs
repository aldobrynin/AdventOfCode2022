namespace AoC2023.Day24;

public static partial class Day24 {
    public static IEnumerable<SampleInput> GetSamples() {
        yield return SampleInput.ForInput(
                """
                19, 13, 30 @ -2,  1, -2
                18, 19, 22 @ -1, -1, -2
                20, 25, 34 @ -2, -2, -4
                12, 31, 28 @ -1, -2, -1
                20, 19, 15 @  1, -5, -3
                """)
            .WithPartOneAnswer(2)
            .WithPartTwoAnswer(47);
    }
}