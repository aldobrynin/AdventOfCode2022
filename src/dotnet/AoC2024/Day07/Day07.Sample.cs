namespace AoC2024.Day07;

public static partial class Day07 {
    public static IEnumerable<SampleInput> GetSamples() {
        yield return SampleInput.ForInput(
                """
                190: 10 19
                3267: 81 40 27
                83: 17 5
                156: 15 6
                7290: 6 8 6 15
                161011: 16 10 13
                192: 17 8 14
                21037: 9 7 18 13
                292: 11 6 16 20
                """
            )
            .WithPartOneAnswer(3749)
            .WithPartTwoAnswer(11387);
    }
}
