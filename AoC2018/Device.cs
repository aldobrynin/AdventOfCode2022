namespace AoC2018;

public record Device(int[] Registers) {
    public static Device New(int registerCount) => new(new int[registerCount]);
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

    public Device Copy() => new(Registers.ToArray());

    public virtual bool Equals(Device? other) {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        return Registers.SequenceEqual(other.Registers);
    }

    public override int GetHashCode() {
        return Registers.Aggregate(new HashCode(), (hash, value) => {
            hash.Add(value);
            return hash;
        }).ToHashCode();
    }

    public override string ToString() => Registers.StringJoin();
}
