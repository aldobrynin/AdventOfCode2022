namespace AoC2023.Day25;

public static partial class Day25 {
    public static IEnumerable<SampleInput> GetSamples() {
        yield return SampleInput.ForInput(
            """
            jqt: rhn xhk nvd
            rsh: frs pzl lsr
            xhk: hfx
            cmg: qnr nvd lhk bvb
            rhn: xhk bvb hfx
            bvb: xhk hfx
            pzl: lsr hfx nvd
            qnr: nvd
            ntq: jqt hfx bvb xhk
            nvd: lhk
            lsr: lhk
            rzs: qnr cmg lsr rsh
            frs: qnr lhk lsr
            """
        ).WithPartOneAnswer(54);
    }
}