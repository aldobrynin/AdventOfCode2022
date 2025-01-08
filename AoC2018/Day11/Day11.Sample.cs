namespace AoC2018.Day11;

public static partial class Day11 {
    public static IEnumerable<SampleInput> GetSamples() {
        yield return SampleInput.ForInput("18")
            .WithPartOneAnswer("33,45")
            .WithPartTwoAnswer("90,269,16");

        yield return SampleInput.ForInput("42")
            .WithPartOneAnswer("21,61")
            .WithPartTwoAnswer("232,251,12");
    }
}
