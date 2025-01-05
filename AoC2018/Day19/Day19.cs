using System.Diagnostics.CodeAnalysis;
using Common.AoC;

namespace AoC2018.Day19;

[SuppressMessage("ReSharper", "StringLiteralTypo")]
public static partial class Day19 {
    record Device(int[] Registers) {
        public static Device From(int[] registers) => new([..registers]);

        public int GetRegisterValue(int register) {
            ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(register, Registers.Length);
            return Registers[register];
        }

        public Device SetRegisterValue(int register, int value) {
            ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(register, Registers.Length);
            Registers[register] = value;
            return this;
        }

        public Device Apply(Instruction instruction) {
            var (opcode, inputA, inputB, inputC) = instruction;
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

        public override string ToString() => Registers.StringJoin();
    }

    record Instruction(string Opcode, int A, int B, int C) {
        public static Instruction Parse(string line) {
            var parts = line.Split(' ');
            return new Instruction(Opcode: parts[0], A: parts[1].ToInt(), B: parts[2].ToInt(), C: parts[3].ToInt());
        }
    }

    public static void Solve(IEnumerable<string> input) {
        var inputs = input.ToArray();
        var ip = inputs[0].Split(' ')[1].ToInt();
        var instructions = inputs.Skip(1).Select(Instruction.Parse).ToArray();

        var registers = new int[6];
        Run(ip, instructions, Device.From(registers))
            .GetRegisterValue(0)
            .Part1();
        FastSolve(0).Part1();

        if (AoCContext.IsSample) return;

        FastSolve(1).Part2();

        RunOptimized(ip, instructions, Device.From(registers).SetRegisterValue(0, 1))
            .GetRegisterValue(0)
            .Part2();

        long FastSolve(int a) {
            var b = a == 0 ? 860 : 10551260;
            return 1.RangeTo(b).Where(d => b % d == 0).Sum();
        }
    }

    private static Device Run(int ip, Instruction[] instructions, Device device) {
        while (device.GetRegisterValue(ip) < instructions.Length) {
            device.Apply(instructions[device.GetRegisterValue(ip)]);
            device.SetRegisterValue(ip, device.GetRegisterValue(ip) + 1);
        }

        return device;
    }

    private static Device RunOptimized(int ip, Instruction[] instructions, Device device) {
        while (device.GetRegisterValue(ip) < instructions.Length) {
            if (device.GetRegisterValue(ip) == 3) {
                if (device.GetRegisterValue(1) % device.GetRegisterValue(3) == 0) {
                    device.SetRegisterValue(0, device.GetRegisterValue(0) + device.GetRegisterValue(3));
                }
                device.SetRegisterValue(ip, 12);
            }

            device.Apply(instructions[device.GetRegisterValue(ip)]);
            device.SetRegisterValue(ip, device.GetRegisterValue(ip) + 1);
        }

        return device;
    }
}
