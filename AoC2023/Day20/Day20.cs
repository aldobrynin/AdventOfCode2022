using Common.AoC;

namespace AoC2023.Day20;

public static partial class Day20 {
    public static void Solve(IEnumerable<string> input) {
        var modules = ParseInput(input).ToDictionary(x => x.Name);
        // PrintGraph(modules);

        Enumerable.Range(0, 1000)
            .SelectMany(_ => PressButton(modules))
            .CountFrequency(x => x.Pulse)
            .Product(x => x.Value)
            .Part1();

        if (AoCContext.IsSample) return;

        foreach (var pair in modules) pair.Value.Reset();

        var conjunctionBeforeRx = modules.Values.Single(x => x.Out.Contains("rx"));
        var cycleExits = conjunctionBeforeRx.Input;
        var periods = new Dictionary<string, long>();

        var iteration = 1;
        while (periods.Count < cycleExits.Length) {
            foreach (var transmission in PressButton(modules))
                if (transmission.Pulse == Pulse.Low && cycleExits.Contains(transmission.To))
                    periods.TryAdd(transmission.To, iteration);

            iteration++;
        }

        periods.Values.Aggregate(1L, MathHelpers.Lcm).Part2();
    }

    private static IEnumerable<Transmission> PressButton(IDictionary<string, Module> modules) {
        var initial = new Transmission("button", "broadcaster", Pulse.Low);
        var signalsQueue = new Queue<Transmission>();
        signalsQueue.Enqueue(initial);
        while (signalsQueue.TryDequeue(out var current)) {
            yield return current;
            if (!modules.TryGetValue(current.To, out var receiver)) continue;
            foreach (var next in receiver.Receive(current.From, current.Pulse))
                signalsQueue.Enqueue(next);
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

        public abstract IEnumerable<Transmission> Receive(string sender, Pulse pulse);

        public abstract void Reset();
    }

    public enum Pulse {
        Low,
        High
    }

    public class FlipFlop(string name, string[] input, string[] output) : Module(name, input, output) {
        private bool Enabled { get; set; }

        public override void Reset() => Enabled = false;

        public override IEnumerable<Transmission> Receive(string sender, Pulse pulse) {
            if (pulse == Pulse.High) return Enumerable.Empty<Transmission>();
            Enabled = !Enabled;
            return Out.Select(x => new Transmission(Name, x, Enabled ? Pulse.High : Pulse.Low));
        }
    }

    public class Broadcaster(string name, string[] input, string[] output) : Module(name, input, output) {
        public override IEnumerable<Transmission> Receive(string sender, Pulse pulse)
            => Out.Select(recipient => new Transmission(Name, recipient, pulse));

        public override void Reset() {
        }
    }

    public class Conjunction(string name, string[] input, string[] outputs) : Module(name, input, outputs) {
        private Dictionary<string, Pulse> Memory { get; } = input.ToDictionary(x => x, _ => Pulse.Low);

        public override IEnumerable<Transmission> Receive(string sender, Pulse pulse) {
            Memory[sender] = pulse;
            var outPulse = Memory.Values.Any(x => x == Pulse.Low) ? Pulse.High : Pulse.Low;
            return Out.Select(recipient => new Transmission(Name, recipient, outPulse));
        }

        public override void Reset() {
            foreach (var memoryKey in Memory.Keys) Memory[memoryKey] = Pulse.Low;
        }
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

    #endregion
}