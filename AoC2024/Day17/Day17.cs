using Common.AoC;

namespace AoC2024.Day17;

public static partial class Day17 {
    public record Computer(long A, long B, long C, long[] Program);

    public static void Solve(IEnumerable<string> input) {
        var lines = input.ToArray();
        var computer = new Computer(
            long.Parse(lines[0].Split(' ')[^1]),
            long.Parse(lines[1].Split(' ')[^1]),
            long.Parse(lines[2].Split(' ')[^1]),
            lines[4].Split(' ')[^1].ToLongArray()
        );

        Run(computer).StringJoin().Part1();
        if (!AoCContext.IsSample || AoCContext.Answers.Item2 is not null)
            CalculateAForOutput(computer, computer.Program).Part2();
    }

    private static IEnumerable<long> Run(Computer computer) {
        var pointer = 0;
        while (pointer < computer.Program.Length) {
            var instruction = computer.Program[pointer];
            var literalOperand = computer.Program[pointer + 1];
            var comboOperand = literalOperand switch {
                <= 3 => literalOperand,
                4 => computer.A,
                5 => computer.B,
                6 => computer.C,
                _ => throw new ArgumentOutOfRangeException("Invalid operand: " + literalOperand)
            };

            computer = instruction switch {
                0 => computer with { A = computer.A >> (int)comboOperand },
                1 => computer with { B = computer.B ^ literalOperand },
                2 => computer with { B = comboOperand % 8 },
                3 => computer,
                4 => computer with { B = computer.B ^ computer.C },
                5 => computer,
                6 => computer with { B = computer.A >> (int)comboOperand },
                7 => computer with { C = computer.A >> (int)comboOperand },
                _ => throw new ArgumentOutOfRangeException("Invalid instruction: " + instruction)
            };

            pointer = instruction == 3 && computer.A != 0
                ? (int)literalOperand
                : pointer + 2;

            if (instruction == 5)
                yield return comboOperand % 8;
        }
    }

    private static long CalculateAForOutput(Computer c, long[] target) {
        var initialA = target.Length == 1
            ? 0
            : CalculateAForOutput(c, target[1..]) << 3;

        return Enumerable.Range(0, 1000)
            .Select(i => c with { A = initialA + i })
            .First(computer => Run(computer).SequenceEqual(target))
            .A;
    }
}

