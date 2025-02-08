namespace AoC2023.Day20;

public static partial class Day20 {
    public static IEnumerable<SampleInput> GetSamples() {
        yield return SampleInput.ForInput(
            """
            broadcaster -> a, b, c
            %a -> b
            %b -> c
            %c -> inv
            &inv -> a
            """
        ).WithPartOneAnswer(32000000);
        
        yield return SampleInput.ForInput(
            """
            broadcaster -> a
            %a -> inv, con
            &inv -> b
            %b -> con
            &con -> output
            """
        ).WithPartOneAnswer(11687500);
    }
}