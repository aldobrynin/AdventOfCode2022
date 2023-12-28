namespace AoC2019.Day05;

public static partial class Day05 {
    public static void Solve(IEnumerable<string> input) {
        var instructions = input.Single();
        Execute(instructions.ToIntArray(), input: 1).Last().Part1();
        Execute(instructions.ToIntArray(), input: 5).Last().Part2();
    }

    private static IEnumerable<int> Execute(int[] instructions, int input) {
        var i = 0;
        while (i < instructions.Length) {
            var opcode = ReadNext(isImmediateMode: true);
            var (modes, op) = Math.DivRem(opcode, 100);
            var mode1 = modes % 10 == 1;
            var mode2 = modes / 10 % 10 == 1;
            if (op == 1) {
                var firstOperand = ReadNext(mode1);
                var secondOperand = ReadNext(mode2);
                instructions[ReadNext()] = firstOperand + secondOperand;
            }
            else if (op == 2) {
                var firstOperand = ReadNext(mode1);
                var secondOperand = ReadNext(mode2);
                instructions[ReadNext()] = firstOperand * secondOperand;
            }
            else if (op == 3) {
                instructions[ReadNext()] = input;
            }
            else if (op == 4) {
                yield return instructions[ReadNext()];
            }
            else if (op == 5) {
                var firstOperand = ReadNext(mode1);
                var secondOperand = ReadNext(mode2);
                if (firstOperand != 0) i = secondOperand;
            }
            else if (op == 6) {
                var firstOperand = ReadNext(mode1);
                var secondOperand = ReadNext(mode2);
                if (firstOperand == 0) i = secondOperand;
            }
            else if (op == 7) {
                var firstOperand = ReadNext(mode1);
                var secondOperand = ReadNext(mode2);
                instructions[ReadNext()] = firstOperand < secondOperand ? 1 : 0;
            }
            else if (op == 8) {
                var firstOperand = ReadNext(mode1);
                var secondOperand = ReadNext(mode2);
                instructions[ReadNext()] = firstOperand == secondOperand ? 1 : 0;
            }
            else if (op == 99) break;
            else throw new Exception($"Unexpected opcode {op} at position {i}");
        }

        int ReadNext(bool isImmediateMode = true) {
            var value = instructions[i++];
            return isImmediateMode ? value : instructions[value];
        }
    }
}