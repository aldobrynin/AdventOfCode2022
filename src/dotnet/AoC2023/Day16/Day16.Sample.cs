namespace AoC2023.Day16;

public static partial class Day16 {
    public static IEnumerable<SampleInput> GetSamples() {
        yield return SampleInput.ForInput(
            """
            .|...\....
            |.-.\.....
            .....|-...
            ........|.
            ..........
            .........\
            ..../.\\..
            .-.-/..|..
            .|....-|.\
            ..//.|....
            """
        ).WithPartOneAnswer(46).WithPartTwoAnswer(51);
    }
}