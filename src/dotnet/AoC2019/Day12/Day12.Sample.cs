namespace AoC2019.Day12;

public static partial class Day12 {
    public static IEnumerable<SampleInput> GetSamples() {
        yield return SampleInput.ForInput(
            """
            <x=-8, y=-10, z=0>
            <x=5, y=5, z=10>
            <x=2, y=-7, z=3>
            <x=9, y=-8, z=-3>
            """
        ).WithPartOneAnswer(1940).WithPartTwoAnswer(4686774924);
    }
}