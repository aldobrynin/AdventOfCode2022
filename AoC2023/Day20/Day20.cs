using Common.AoC;

namespace AoC2023.Day20;

public static partial class Day20 {
    public static void Solve(IEnumerable<string> input) {
        var modules = ParseInput(input).ToDictionary(x => x.Name);
        // PrintGraph(modules);

        Enumerable.Range(0, 1000).SelectMany(_ => PressButton(modules))
            .GroupBy(x => x.Pulse)
            .Product(x => x.LongCount())
            .Part1();

        if (AoCContext.IsSample) return;

        foreach (var pair in modules) pair.Value.Reset();

        var conjunctionBeforeOut = modules.Values.Single(x => x.Out.Contains("rx"));
        var cycleExits = conjunctionBeforeOut.Input;
        var periods = new Dictionary<string, long>();
        for (var i = 1;; i++) {
            foreach (var transmission in PressButton(modules)) {
                if (transmission.Pulse == Pulse.Low && cycleExits.Contains(transmission.To))
                    periods.TryAdd(transmission.To, i);
            }

            if (periods.Count == cycleExits.Length) break;
        }

        periods.Values.Aggregate(1L, MathHelpers.Lcm).Part2();
    }

    private static void PrintGraph(IDictionary<string, Module> modules) {
        Console.WriteLine("digraph G {");
        foreach (var module in modules.Values) {
            var shape = module switch {
                FlipFlop => "box",
                Conjunction => "diamond",
                _ => "ellipse",
            };
            Console.WriteLine($"  {module.Name} [shape={shape}]");
            Console.WriteLine($"  {module.Name} -> {module.Out.StringJoin()}");
        }

        Console.WriteLine("}");
    }

    private static IEnumerable<Transmission> PressButton(IDictionary<string, Module> modules) {
        var initial = new Transmission("button", "broadcaster", Pulse.Low);
        var nextSignals = new List<Transmission> { initial };
        while (nextSignals.Count != 0) {
            var next = new List<Transmission>();
            foreach (var signal in nextSignals) {
                // Console.WriteLine($"{signal.From} -{signal.Pulse}-> {signal.To}");
                yield return signal;
                if (modules.TryGetValue(signal.To, out var receiver))
                    next.AddRange(receiver.Receive(signal.From, signal.Pulse));
            }

            nextSignals = next;
        }
    }

    private static Module[] ParseInput(IEnumerable<string> input) {
        var modules = input.Select(line => {
            var parts = line.Split(" -> ");
            var type = parts[0][0] is '%' or '&' ? parts[0][0].ToString() : "";
            return (
                Type: type,
                Name: parts[0].TrimStart('%', '&'),
                Out: parts[1].Split(',').Select(x => x.Trim()).ToArray());
        }).ToArray();
        var inputPerModule = modules.SelectMany(kv => kv.Out.Select(x => (Out: x, In: kv.Name)))
            .ToLookup(x => x.Out, x => x.In);
        return modules.Select(x => x.Type switch {
                "%" => (Module)new FlipFlop(x.Name, inputPerModule[x.Name].ToArray(), x.Out),
                "&" => new Conjunction(x.Name, inputPerModule[x.Name].ToArray(), x.Out),
                _ => new Broadcaster(x.Name, inputPerModule[x.Name].ToArray(), x.Out)
            })
            .ToArray();
    }

    #region Types

    public record Transmission(string From, string To, Pulse Pulse);

    public abstract class Module(string name, string[] input, string[] output) {
        public string[] Out => output;
        public string[] Input => input;
        public string Name => name;

        public abstract Transmission[] Receive(string sender, Pulse pulse);

        public abstract void Reset();
    }

    public enum Pulse {
        Low,
        High
    }

    public class FlipFlop(string name, string[] input, string[] output) : Module(name, input, output) {
        public bool Enabled { get; set; }

        public override void Reset() => Enabled = false;

        public override Transmission[] Receive(string sender, Pulse pulse) {
            if (pulse == Pulse.High) return Array.Empty<Transmission>();
            Enabled = !Enabled;
            return Out.Select(x => new Transmission(Name, x, Enabled ? Pulse.High : Pulse.Low)).ToArray();
        }
    }

    public class Broadcaster(string name, string[] input, string[] output) : Module(name, input, output) {
        public override Transmission[] Receive(string sender, Pulse pulse) {
            return Out.Select(x => new Transmission(Name, x, pulse)).ToArray();
        }

        public override void Reset() {
        }
    }

    public class Conjunction(string name, string[] input, string[] outputs) : Module(name, input, outputs) {
        private Dictionary<string, Pulse> Memory { get; } = input.ToDictionary(x => x, _ => Pulse.Low);

        public override Transmission[] Receive(string sender, Pulse pulse) {
            Memory[sender] = pulse;
            var allHigh = Memory.Values.All(x => x == Pulse.High);
            return Out.Select(x => new Transmission(Name, x, allHigh ? Pulse.Low : Pulse.High)).ToArray();
        }

        public override void Reset() {
            foreach (var memoryKey in Memory.Keys) Memory[memoryKey] = Pulse.Low;
        }
    }

    #endregion
}