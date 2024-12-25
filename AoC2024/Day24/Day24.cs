using Common.AoC;

namespace AoC2024.Day24;

public static partial class Day24 {
    public record Gate(string[] Inputs, string Output, string Operation) {
        public string Output { get; set; } = Output;

        public override string ToString() => $"<{Inputs[0]} {Operation} {Inputs[1]} -> {Output}>";

        public bool HasInput(string gate) => Inputs.Contains(gate);
    }

    public static void Solve(IEnumerable<string> input) {
        var sections = input.SplitBy(string.IsNullOrWhiteSpace).ToArray();
        var values = sections[0]
            .Select(x => x.Split(": "))
            .ToDictionary(x => x[0], x => x[1].ToInt());

        var gates = sections[1]
            .Select(x => x.Split(' '))
            .Select(line => new Gate([line[0], line[2]], line[^1], line[1]))
            .ToArray();

        ReadRegisterValue(GetOutput(gates, values), 'z').Part1();

        var expectedZ = ReadRegisterValue(values, 'x') + ReadRegisterValue(values, 'y');

        if (!AoCContext.IsSample) {
            var res = FixWires(gates, values).ToArray();
            if (ReadRegisterValue(GetOutput(gates, values), 'z') != expectedZ)
                throw new Exception("Failed to fix the circuit");

            res.SelectMany(x => new[] { x.Item1, x.Item2 })
                .Order()
                .StringJoin()
                .Part2();
        }
    }

    private static IEnumerable<(string, string)> FixWires(Gate[] gates, Dictionary<string, int> inputs) {
        Gate? carry = null;
        var bits = inputs.Count(x => x.Key.StartsWith("x"));
        for (var i = 0; i < bits; i++) {
            var inputX = $"x{i:00}";
            var inputY = $"y{i:00}";
            var outputZ = $"z{i:00}";
            var zGate = gates.Single(x => x.Output == outputZ);

            var xyLowerBit = GetGateOrThrow(inputX, inputY, "XOR");
            var xyHigherBit = GetGateOrThrow(inputX, inputY, "AND");

            if (carry is null) {
                carry = xyHigherBit;
                continue;
            }

            if (!zGate.HasInput(carry.Output)) {
                zGate.Dump();
                var correctGate = GetGateByInputAndOperation(carry.Output, "XOR");
                (correctGate.Output, zGate.Output) = (zGate.Output, correctGate.Output);
                yield return (correctGate.Output, zGate.Output);
                zGate = correctGate;
            }


            if (!zGate.HasInput(xyLowerBit.Output)) {
                var newOutput = zGate.Inputs.Single(x => x != carry.Output);
                var gateToFix = gates.Single(x => x.Output == newOutput);
                (xyLowerBit.Output, gateToFix.Output) = (gateToFix.Output, xyLowerBit.Output);
                yield return (xyLowerBit.Output, gateToFix.Output);
            }

            var lowerBitWithCarry = GetGateOrThrow(carry.Output, xyLowerBit.Output, "XOR");
            if (lowerBitWithCarry.Output != outputZ) {
                (lowerBitWithCarry.Output, zGate.Output) = (zGate.Output, lowerBitWithCarry.Output);
                yield return (lowerBitWithCarry.Output, zGate.Output);
            }

            var higherBitWithCarry = GetGateOrThrow(carry.Output, xyLowerBit.Output, "AND");
            carry = GetGateOrThrow(xyHigherBit.Output, higherBitWithCarry.Output, "OR");
        }

        Gate GetGateOrThrow(string input1, string input2, string operation) =>
            GetGateOrNull(input1, input2, operation)
            ?? throw new Exception($"Gate for '{input1} {operation} {input2}' not found");

        Gate? GetGateOrNull(string input1, string input2, string operation) =>
            gates.SingleOrDefault(g => g.Operation == operation && g.HasInput(input1) && g.HasInput(input2));

        Gate GetGateByInputAndOperation(string input, string operation) =>
            gates.SingleOrDefault(g => g.HasInput(input) && g.Operation == operation)
            ?? throw new Exception($"Gate for '{input} {operation}' not found");
    }

    private static Dictionary<string, int> GetOutput(Gate[] gates, Dictionary<string, int> values) {
        var visited = new HashSet<string>();
        var outputs = new Dictionary<string, int>(values);

        var queue = new Queue<Gate>(gates.Where(x => x.Inputs.All(values.ContainsKey)));
        visited.UnionWith(queue.Select(x => x.Output));
        while (queue.TryDequeue(out var current)) {
            outputs[current.Output] = current.Operation switch {
                "AND" => current.Inputs.Select(x => outputs[x]).Aggregate((a, b) => a & b),
                "OR" => current.Inputs.Select(x => outputs[x]).Aggregate((a, b) => a | b),
                "XOR" => current.Inputs.Select(x => outputs[x]).Aggregate((a, b) => a ^ b),
                _ => throw new ArgumentOutOfRangeException(nameof(current.Operation), current.Operation, "Invalid operation"),
            };

            foreach (var gate in gates
                         .Where(x => !outputs.ContainsKey(x.Output) && x.Inputs.All(outputs.ContainsKey))
                         .Where(x => visited.Add(x.Output))
                    ) {
                queue.Enqueue(gate);
            }
        }

        return outputs;
    }

    private static long ReadRegisterValue(Dictionary<string, int> output, char register) =>
        output
            .Where(x => x.Key.StartsWith(register))
            .OrderByDescending(x => x.Key)
            .Aggregate(0L, (acc, current) => (acc << 1) + current.Value);
}
