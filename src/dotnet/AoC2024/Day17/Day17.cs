using Common.AoC;

namespace AoC2024.Day17;

public static partial class Day17 {
    public record Computer(long A, long B, long C, long[] Program, int Pointer = 0) {
        public bool IsRunning => Pointer < Program.Length;

        public Instruction Instruction => (Instruction)Program[Pointer];
        public long LiteralOperand => Program[Pointer + 1];
        public long ComboOperand => LiteralOperand switch {
            <= 3 => LiteralOperand,
            4 => A,
            5 => B,
            6 => C,
            _ => throw new ArgumentOutOfRangeException(nameof(LiteralOperand), LiteralOperand, "Invalid operand")
        };

        public Computer Next() {
            var instruction = (Instruction)Program[Pointer];
            return instruction switch {
                Instruction.Adv => this with { A = A >> (int)ComboOperand, Pointer = Pointer + 2 },
                Instruction.Bxl => this with { B = B ^ LiteralOperand, Pointer = Pointer + 2 },
                Instruction.Bst => this with { B = ComboOperand & 0b111, Pointer = Pointer + 2 },
                Instruction.Jnz when A is 0 => this with { Pointer = Pointer + 2 },
                Instruction.Jnz when A is not 0 => this with { Pointer = (int)LiteralOperand },
                Instruction.Bxc => this with { B = B ^ C, Pointer = Pointer + 2 },
                Instruction.Out => this with { Pointer = Pointer + 2 },
                Instruction.Bdv => this with { B = A >> (int)ComboOperand, Pointer = Pointer + 2 },
                Instruction.Cdv => this with { C = A >> (int)ComboOperand, Pointer = Pointer + 2 },
                _ => throw new ArgumentOutOfRangeException(nameof(instruction), instruction, "Invalid instruction"),
            };
        }
    }

    public enum Instruction {
        Adv = 0,
        Bxl = 1,
        Bst = 2,
        Jnz = 3,
        Bxc = 4,
        Out = 5,
        Bdv = 6,
        Cdv = 7,
    }

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
        while (computer.IsRunning) {
            if (computer.Instruction is Instruction.Out)
                yield return computer.ComboOperand & 0b111;
            computer = computer.Next();
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

