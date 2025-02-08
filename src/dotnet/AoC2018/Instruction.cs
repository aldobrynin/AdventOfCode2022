using System.Text;

namespace AoC2018;

public record Instruction(string Opcode, int A, int B, int C) {
    public static Instruction Parse(string line) {
        var parts = line.Split(' ');
        return new Instruction(Opcode: parts[0], A: parts[1].ToInt(), B: parts[2].ToInt(), C: parts[3].ToInt());
    }

    public override string ToString() {
        var sb = new StringBuilder();
        char ToChar(int i) => (char)('a' + i);

        sb.Append(ToChar(C));
        sb.Append(" = ");

        var expression = Opcode switch {
            "addr" => $"{ToChar(A)} + {ToChar(B)}",
            "addi" => $"{ToChar(A)} + {B}",
            "mulr" => $"{ToChar(A)} * {ToChar(B)}",
            "muli" => $"{ToChar(A)} * {B}",
            "banr" => $"{ToChar(A)} & {ToChar(B)}",
            "bani" => $"{ToChar(A)} & {B}",
            "borr" => $"{ToChar(A)} | {ToChar(B)}",
            "bori" => $"{ToChar(A)} | {B}",
            "setr" => $"{ToChar(A)}",
            "seti" => $"{A}",
            "gtir" => $"{A} > {ToChar(B)} ? 1 : 0",
            "gtri" => $"{ToChar(A)} > {B} ? 1 : 0",
            "gtrr" => $"{ToChar(A)} > {ToChar(B)} ? 1 : 0",
            "eqir" => $"{A} == {ToChar(B)} ? 1 : 0",
            "eqri" => $"{ToChar(A)} == {B} ? 1 : 0",
            "eqrr" => $"{ToChar(A)} == {ToChar(B)} ? 1 : 0",
            _ => throw new InvalidOperationException($"Unknown opcode: {Opcode}")
        };

        sb.Append(expression);
        return sb.ToString();
    }
}
