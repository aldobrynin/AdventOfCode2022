using System.Diagnostics.CodeAnalysis;
using Common.AoC;

namespace AoC2018.Day19;

[SuppressMessage("ReSharper", "StringLiteralTypo")]
public static partial class Day19 {
    public static void Solve(IEnumerable<string> input) {
        var inputs = input.ToArray();
        var ip = inputs[0].Split(' ')[1].ToInt();
        var instructions = inputs.Skip(1).Select(Instruction.Parse).ToArray();

        Run(ip, instructions, Device.New(registerCount: 6))
            .GetRegisterValue(0)
            .Part1();
        FastSolve(0).Part1();

        if (AoCContext.IsSample) return;

        FastSolve(1).Part2();

        RunOptimized(ip, instructions, Device.New(registerCount: 6).SetRegisterValue(0, 1))
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
