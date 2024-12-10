namespace AoC2024.Day10;

public static partial class Day10 {
    public static IEnumerable<SampleInput> GetSamples() {
        yield return SampleInput.ForInput(
                """
                89010123
                78121874
                87430965
                96549874
                45678903
                32019012
                01329801
                10456732
                """
            )
            .WithPartOneAnswer(36)
            .WithPartTwoAnswer(81);
    }
}
