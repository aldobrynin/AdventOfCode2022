namespace AoC2018.Day21;

public static partial class Day21 {
    public static void Solve(IEnumerable<string> input) {
        var lines = input.ToArray();
        var ip = lines.First().Split(' ')[^1].ToInt();
        var instructions = lines.Skip(1).Select(Instruction.Parse).ToArray();

        FindFirst(RunCode()).Part1();
        FindFirst(Simulate(ip, instructions)).Part1();

        FindEndOfCycle(RunCode()).Part2();
        FindEndOfCycle(Simulate(ip, instructions)).Part2();

        long FindFirst(IEnumerable<long> source) => source.First();

        long FindEndOfCycle(IEnumerable<long> source) {
            var history = new HashSet<long>();
            return source.TakeWhile(history.Add).Last();
        }
    }

    private static IEnumerable<long> Simulate(int ip, Instruction[] instructions) =>
        Apply(ip, instructions, Device.New(registerCount: 6));

    private static IEnumerable<long> Apply(int ip, Instruction[] instructions, Device device) {
        while (device.GetRegisterValue(ip) < instructions.Length) {
            var instructionIndex = device.GetRegisterValue(ip);
            // optimize hotspot
            if (instructionIndex == 17) {
                device.SetRegisterValue(3, device.GetRegisterValue(3) / 256);
                device.SetRegisterValue(ip, 8);
            }
            else {
                var instruction = instructions[instructionIndex];
                if (instructionIndex == 28)
                    yield return device.GetRegisterValue(instruction.A);

                device.Apply(instruction);
                device.SetRegisterValue(ip, device.GetRegisterValue(ip) + 1);
            }
        }
    }

    /*
#ip 5
00: e = 123
01: e = e & 456
02: e = e == 72 ? 1 : 0
03: f = e + f
04: f = 0
05: e = 0
06: d = e | 65536
07: e = 14464005
08: c = d & 255
09: e = e + c
10: e = e & 16777215
11: e = e * 65899
12: e = e & 16777215
13: c = 256 > d ? 1 : 0
14: f = c + f // if 256 > d goto 28 else goto 17
15: f = f + 1
16: f = 27
17: c = 0
18: b = c + 1
19: b = b * 256
20: b = b > d ? 1 : 0
21: f = b + f // if b > d d=c and goto 8 else c++ and goto 18
22: f = f + 1
23: f = 25
24: c = c + 1
25: f = 17
26: d = c
27: f = 7
28: c = e == a ? 1 : 0
29: f = c + f // if e == a HALT else goto 6
30: f = 5
     */
    private static IEnumerable<long> RunCode(long a = 0) {
        long e = 0;
        do {
            var d = e | 0x10000;
            e = 14464005;

            do {
                e = (((e + (d & 0xFF)) & 0xFFFFFF) * 65899) & 0xFFFFFF;
                d /= 256;
            } while (d > 0);

            yield return e;
        } while (e != a);
    }
}
