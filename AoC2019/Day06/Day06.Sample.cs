namespace AoC2019.Day06;

public static partial class Day06 {
    public static IEnumerable<SampleInput> GetSamples() {
        yield return SampleInput.ForInput(
            """
            COM)B
            B)C
            C)D
            D)E
            E)F
            B)G
            G)H
            D)I
            E)J
            J)K
            K)L
            """
        ).WithPartOneAnswer(42);

        yield return SampleInput.ForInput(
            """
            COM)B
            B)C
            C)D
            D)E
            E)F
            B)G
            G)H
            D)I
            E)J
            J)K
            K)L
            K)YOU
            I)SAN
            """
        ).WithPartOneAnswer(54).WithPartTwoAnswer(4);
    }
}