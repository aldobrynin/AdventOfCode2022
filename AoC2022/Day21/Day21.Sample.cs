namespace AoC2022.Day21;

public partial class Day21 {
    public static IEnumerable<SampleInput> GetSamples() {
        yield return SampleInput.ForInput("""
root: pppw + sjmn
dbpl: 5
cczh: sllz + lgvd
zczc: 2
ptdq: humn - dvpt
dvpt: 3
lfqf: 4
humn: 5
ljgn: 2
sjmn: drzm * dbpl
sllz: 4
pppw: cczh / lfqf
lgvd: ljgn * ptdq
drzm: hmdt - zczc
hmdt: 32
""")
            .WithPartOneAnswer("152")
            .WithPartTwoAnswer("301");
    }
}