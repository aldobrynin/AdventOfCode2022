namespace AoC2021.Day10;

public partial class Day10 {
    public static IEnumerable<SampleInput> GetSamples() {
        yield return SampleInput.ForInput("""
                                          [({(<(())[]>[[{[]{<()<>>
                                          [(()[<>])]({[<{<<[]>>(
                                          {([(<{}[<>[]}>{[]{[(<()>
                                          (((({<>}<{<{<>}{[]{[]{}
                                          [[<[([]))<([[{}[[()]]]
                                          [{[{({}]{}}([{[{{{}}([]
                                          {<[[]]>}<{[{[{[]{()[[[]
                                          [<(<(<(<{}))><([]([]()
                                          <{([([[(<>()){}]>(<<{{
                                          <{([{{}}[<[[[<>{}]]]>[]]
                                          """)
            .WithPartOneAnswer("26397")
            .WithPartTwoAnswer("288957");
    }
}