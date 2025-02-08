namespace AoC2018.Day07;

public static partial class Day07 {
    public static IEnumerable<SampleInput> GetSamples() {
        yield return SampleInput.ForInput("""
                                          Step C must be finished before step A can begin.
                                          Step C must be finished before step F can begin.
                                          Step A must be finished before step B can begin.
                                          Step A must be finished before step D can begin.
                                          Step B must be finished before step E can begin.
                                          Step D must be finished before step E can begin.
                                          Step F must be finished before step E can begin.
                                          """)
            .WithPartOneAnswer("CABDFE")
            .WithPartTwoAnswer(15);
    }
}
