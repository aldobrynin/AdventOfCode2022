namespace AoC2019.Day03;

public static partial class Day03 {
    public static IEnumerable<SampleInput> GetSamples() {
        yield return SampleInput.ForInput(
            """
            R8,U5,L5,D3
            U7,R6,D4,L4
            """
        ).WithPartOneAnswer(6).WithPartTwoAnswer(30);


        yield return SampleInput.ForInput(
            """
            R75,D30,R83,U83,L12,D49,R71,U7,L72
            U62,R66,U55,R34,D71,R55,D58,R83
            """
        ).WithPartOneAnswer(159).WithPartTwoAnswer(610);
    }
}