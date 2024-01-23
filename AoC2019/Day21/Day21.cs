namespace AoC2019.Day21;

public static partial class Day21 {
    public static void Solve(IEnumerable<string> input) {
        var program = input.Single().ToLongArray();
        const string walkInstructions = """
                                        OR A T
                                        AND B T
                                        AND C T
                                        NOT T T
                                        AND D T
                                        NOT T T
                                        NOT T J
                                        WALK

                                        """;
        Run(program, walkInstructions).Part1();
        const string runInstructions = """
                                       OR A T
                                       AND B T
                                       AND C T
                                       NOT T T
                                       AND D T
                                       NOT T T
                                       NOT T J
                                       NOT E T
                                       NOT T T
                                       OR H T
                                       AND T J
                                       RUN

                                       """;
        Run(program, runInstructions).Part2();
    }

    private static long Run(long[] intCodeProgram, string instructions) {
        var computer = new IntCodeComputer(intCodeProgram, instructions.Select(x => (long)x).ToArray());
        return computer.ReadAllOutputs().Last();
        // foreach (var output in computer.ReadAllOutputs()) {
        //     if (output > char.MaxValue) return output;
        //     Console.Out.Write((char)output);
        // }
        // throw new InvalidOperationException("Should not reach here");
    }
}