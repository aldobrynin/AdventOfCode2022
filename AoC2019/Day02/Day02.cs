using Common.AoC;

namespace AoC2019.Day02;

public static partial class Day02 {
    public static void Solve(IEnumerable<string> input) {
        var program = input.Single().ToIntArray();

        var result = Evaluate(program, 12, 2);

        if (AoCContext.IsSample) result.StringJoin().Part1();
        else result[0].Part1();

        0.RangeTo(99).SelectMany(noun => 0.RangeTo(99).Select(verb => (noun, verb)))
            .Where(x => Evaluate(program, x.noun, x.verb)[0] == 19690720)
            .Select(x => x.noun * 100 + x.verb)
            .SingleOrDefault()
            .Part2();
    }

    private static int[] Evaluate(int[] input, int noun, int verb) {
        var program = input.ToArray();
        if (!AoCContext.IsSample) {
            program[1] = noun;
            program[2] = verb;
        }

        foreach (var block in program.Chunk(4)) {
            if (block[0] == 99) break;
            program[block[3]] = block[0] switch {
                1 => program[block[1]] + program[block[2]],
                2 => program[block[1]] * program[block[2]],
                _ => throw new ArgumentOutOfRangeException($"Unexpected opcode {block[0]}")
            };
        }

        return program;
    }
}