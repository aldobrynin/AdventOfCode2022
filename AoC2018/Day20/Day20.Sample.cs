namespace AoC2018.Day20;

public static partial class Day20 {
    public static IEnumerable<SampleInput> GetSamples() {
        yield return SampleInput.ForInput("^ENWWW(NEEE|SSE(EE|N))$")
            .WithPartOneAnswer(10);

        yield return SampleInput.ForInput("^ESSWWN(E|NNENN(EESS(WNSE|)SSS|WWWSSSSE(SW|NNNE)))$")
            .WithPartOneAnswer(23);

        yield return SampleInput.ForInput("^WSSEESWWWNW(S|NENNEEEENN(ESSSSW(NWSW|SSEN)|WSWWN(E|WWS(E|SS))))$")
            .WithPartOneAnswer(31);
    }
}
