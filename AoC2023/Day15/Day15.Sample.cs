namespace AoC2023.Day15;

public static partial class Day15 {
    public static IEnumerable<SampleInput> GetSamples() {
        yield return SampleInput.ForInput("rn=1,cm-,qp=3,cm=2,qp-,pc=4,ot=9,ab=5,pc-,pc=6,ot=7")
            .WithPartOneAnswer(1320).WithPartTwoAnswer(145);
    }
}