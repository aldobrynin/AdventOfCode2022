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

    record Device(int Register0, int Register1, int Register2, int Register3) {
        public static readonly Device Empty = new(0, 0, 0, 0);
        public static Device From(int[] registers) => new(registers[0], registers[1], registers[2], registers[3]);

        private int GetRegisterValue(int register) => register switch {
            0 => Register0,
            1 => Register1,
            2 => Register2,
            3 => Register3,
            _ => throw new InvalidOperationException($"Unknown register: {register}")
        };

        private Device SetRegisterValue(int register, int value) => register switch {
            0 => this with { Register0 = value },
            1 => this with { Register1 = value },
            2 => this with { Register2 = value },
            3 => this with { Register3 = value },
            _ => throw new InvalidOperationException($"Unknown register: {register}")
        };

        public Device Apply(string opcode, int inputA, int inputB, int inputC) {
            var result = opcode switch {
                "addr" => GetRegisterValue(inputA) + GetRegisterValue(inputB),
                "addi" => GetRegisterValue(inputA) + inputB,
                "mulr" => GetRegisterValue(inputA) * GetRegisterValue(inputB),
                "muli" => GetRegisterValue(inputA) * inputB,
                "banr" => GetRegisterValue(inputA) & GetRegisterValue(inputB),
                "bani" => GetRegisterValue(inputA) & inputB,
                "borr" => GetRegisterValue(inputA) | GetRegisterValue(inputB),
                "bori" => GetRegisterValue(inputA) | inputB,
                "setr" => GetRegisterValue(inputA),
                "seti" => inputA,
                "gtir" => inputA > GetRegisterValue(inputB) ? 1 : 0,
                "gtri" => GetRegisterValue(inputA) > inputB ? 1 : 0,
                "gtrr" => GetRegisterValue(inputA) > GetRegisterValue(inputB) ? 1 : 0,
                "eqir" => inputA == GetRegisterValue(inputB) ? 1 : 0,
                "eqri" => GetRegisterValue(inputA) == inputB ? 1 : 0,
                "eqrr" => GetRegisterValue(inputA) == GetRegisterValue(inputB) ? 1 : 0,
                _ => throw new InvalidOperationException($"Unknown opcode: {opcode}")
            };

            return SetRegisterValue(inputC, result);
        }
    }

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
            .Aggregate(Device.Empty, (current, x) => current.Apply(opcodeMapping[x[0]], x[1], x[2], x[3]))
            .Register0
            .Part2();

        string[] GetValidOpcodes(Device before, int[] program, Device after, IEnumerable<string> candidates)
            => candidates
                .Where(op => before.Apply(op, program[1], program[2], program[3]) == after)
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
