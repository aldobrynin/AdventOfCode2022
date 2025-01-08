using Common.AoC;

namespace AoC2018.Day16;

public static partial class Day16 {
    private static readonly string[] Opcodes = [
        "addr",
        "addi",
        "mulr",
        "muli",
        "banr",
        "bani",
        "borr",
        "bori",
        "setr",
        "seti",
        "gtir",
        "gtri",
        "gtrr",
        "eqir",
        "eqri",
        "eqrr",
    ];

    public static void Solve(IEnumerable<string> input) {
        var blocks = input
            .SplitBy(string.IsNullOrWhiteSpace)
            .ToArray();

        var samples = blocks
            .TakeWhile(x => x.Count > 0 && x[0].StartsWith("Before"))
            .Select(s => {
                var before = s[0].Split(' ', 2, StringSplitOptions.RemoveEmptyEntries)[^1].Trim('[', ']').ToIntArray();
                var program = s[1].ToIntArray();
                var after = s[2].Split(' ', 2, StringSplitOptions.RemoveEmptyEntries)[^1].Trim('[', ']').ToIntArray();
                return (Before: Device.From(before), Program: program, After: Device.From(after));
            })
            .ToArray();

        var testProgram = blocks
            .SkipWhile(x => x.Count == 0 || x[0].StartsWith("Before"))
            .Flatten()
            .Select(x => x.ToIntArray())
            .ToArray();

        samples
            .Count(s => GetValidOpcodes(s.Before, s.Program, s.After, Opcodes).Length >= 3)
            .Part1();

        if(AoCContext.IsSample) return;

        var opcodeMapping = ResolveMapping();

        testProgram
            .Select(program => new Instruction(opcodeMapping[program[0]], program[1], program[2], program[3]))
            .Aggregate(Device.New(registerCount: 4), (current, instruction) => current.Apply(instruction))
            .GetRegisterValue(0)
            .Part2();

        string[] GetValidOpcodes(Device before, int[] program, Device after, IEnumerable<string> candidates)
            => candidates
                .Select(opcode => new Instruction(opcode, program[1], program[2], program[3]))
                .Where(instruction => before.Copy().Apply(instruction) == after)
                .Select(instruction => instruction.Opcode)
                .ToArray();

        Dictionary<int, string> ResolveMapping() {
            var dictionary = new Dictionary<int, string>();
            var unmappedOpcodes = new HashSet<string>(Opcodes);
            while (unmappedOpcodes.Count > 0) {
                var resolved = samples
                    .Where(x => !dictionary.ContainsKey(x.Program[0]))
                    .Select(x => (
                        Code: x.Program[0],
                        Opcodes: GetValidOpcodes(x.Before, x.Program, x.After, unmappedOpcodes))
                    )
                    .Where(x => x.Opcodes.Length == 1)
                    .Select(x => (x.Code, Opcode: x.Opcodes[0]));

                foreach (var (code, opcode) in resolved) {
                    dictionary[code] = opcode;
                    unmappedOpcodes.Remove(opcode);
                }
            }

            return dictionary;
        }
    }
}
