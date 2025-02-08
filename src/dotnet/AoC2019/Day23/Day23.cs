namespace AoC2019.Day23;
using Output = (int Recepient, long X, long Y);

public static partial class Day23 {
    public static void Solve(IEnumerable<string> input) {
        var program = input.Single().ToLongArray();

        var computers = Enumerable.Range(0, 50)
            .Select(address => new IntCodeComputer(program, [address]))
            .ToArray();
        Output? currentNatValue = null;
        foreach (var computer in computers) {
            computer.PipeOutputTo(OutputHandler(output => {
                if (output.Recepient == 255) {
                    if (currentNatValue is null) output.Y.Part1();
                    currentNatValue = output;
                }
                else computers[output.Recepient].AddInput(output.X, output.Y);
            }));
        }

        Task.WhenAll(computers.Select(c => c.RunToEnd()));
        var lastSentNat = (Output?)null;
        while (true) {
            if (computers.All(c => c.IsWaitingForInput) && currentNatValue.HasValue) {
                if (lastSentNat?.Y == currentNatValue?.Y) {
                    lastSentNat!.Value.Y.Part2();
                    break;
                }

                lastSentNat = currentNatValue;
                computers[0].AddInput(currentNatValue!.Value.X, currentNatValue.Value.Y);
            }
            else {
                foreach (var computer in computers.Where(x => x.IsWaitingForInput)) {
                    computer.AddInput(-1);
                }
            }
        }
    }

    private static Action<long> OutputHandler(Action<Output> onOutput) {
        int? recipient = null;
        long? x = null;

        return value => {
            if (recipient is null) recipient = (int)value;
            else if (x is null) x = value;
            else {
                onOutput((recipient.Value, x.Value, value));
                recipient = null;
                x = null;
            }
        };
    }
}