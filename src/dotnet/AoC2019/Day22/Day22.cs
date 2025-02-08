using System.Numerics;

namespace AoC2019.Day22;

public static partial class Day22 {
    private enum InstructionType {
        DealIntoNewStack,
        Cut,
        DealWithIncrement,
    }

    private record Instruction(InstructionType Type, int? N = null) {
        public static Instruction Parse(string line) {
            var parts = line.Split(' ');
            return (parts[0], parts[1]) switch {
                ("deal", "into") => new Instruction(InstructionType.DealIntoNewStack),
                ("deal", "with") => new Instruction(InstructionType.DealWithIncrement, parts[3].ToInt()),
                ("cut", _) => new Instruction(InstructionType.Cut, parts[1].ToInt()),
                _ => throw new Exception($"Unknown instruction: {line}")
            };
        }
    }

    public static void Solve(IEnumerable<string> input) {
        var instructions = input.Select(Instruction.Parse).ToArray();
        instructions.Aggregate(Enumerable.Range(0, 10007).ToArray(),
                (current, instruction) => instruction.Type switch {
                    InstructionType.DealIntoNewStack => current.Reverse().ToArray(),
                    InstructionType.Cut => current.Cut(instruction.N!.Value),
                    InstructionType.DealWithIncrement => current.Deal(instruction.N!.Value),
                    _ => current
                })
            .ToList()
            .IndexOf(2019)
            .Part1();

        Part2(instructions).Part2();
    }

    private static long Part2(IEnumerable<Instruction> input) {
        BigInteger deckSize = 119_315_717_514_047;
        BigInteger iterations = 101_741_582_076_661;
        BigInteger target = 2020;

        var (incrementMul, offsetDiff) = input.Aggregate((BigInteger.One, BigInteger.Zero), Apply);

        var increment = BigInteger.ModPow(incrementMul, iterations, deckSize);
        var offset = offsetDiff * (1 - increment) * (1 - incrementMul).Inverse(deckSize);

        return (long)((increment * target + offset) % deckSize);

        (BigInteger Increment, BigInteger Offset) Apply(
            (BigInteger Increment, BigInteger Offset) current,
            Instruction instruction) {
            var (incr, offs) = current;
            switch (instruction.Type) {
                case InstructionType.DealIntoNewStack:
                    incr *= -1;
                    offs += incr;
                    break;
                case InstructionType.Cut:
                    offs += incr * instruction.N!.Value;
                    break;
                case InstructionType.DealWithIncrement:
                    incr *= new BigInteger(instruction.N!.Value).Inverse(deckSize);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return (incr.Mod(deckSize), offs.Mod(deckSize));
        }
    }

    private static BigInteger Inverse(this BigInteger n, BigInteger mod) => BigInteger.ModPow(n, mod - 2, mod);

    private static int[] Cut(this int[] deck, int n) {
        var index = n.Mod(deck.Length);
        return deck.Skip(index).Concat(deck.Take(index)).ToArray();
    }

    private static int[] Deal(this int[] deck, int n) {
        var result = new int[deck.Length];
        var index = 0;
        foreach (var card in deck) {
            result[index] = card;
            index = (index + n) % deck.Length;
        }

        return result;
    }
}