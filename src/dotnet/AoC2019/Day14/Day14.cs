namespace AoC2019.Day14;

public static partial class Day14 {

    public static void Solve(IEnumerable<string> input) {
        var reactions = input.Select(line => {
            var parts = line.Split(" => ");
            var (quantity, chemical) = Parse(parts[1]);
            return (Chemical: chemical, OutputQuantity: quantity, Inputs: parts[0].Split(", ").Select(Parse).ToArray());
        }).ToDictionary(x => x.Chemical, x => (x.OutputQuantity, x.Inputs));
        FindOreCount().Part1();
        FindFuelQuantity().Part2();

        (int Quantity, string Chemical) Parse(string s) {
            var parts = s.Split(" ");
            return (parts[0].ToInt(), parts[1]);
        }

        long FindOreCount(long fuelQuantity = 1) {
            var required = new Dictionary<string, long> {
                ["FUEL"] = fuelQuantity
            };
            while (true) {
                var next = required.FirstOrDefault(pair => pair.Key != "ORE" && pair.Value > 0);
                if (next.Key == default) break;
                var (quantity, inputs) = reactions[next.Key];
                var multiplier = (long)Math.Ceiling((double)next.Value / quantity);
                foreach (var (inputQuantity, chemical) in inputs)
                    required[chemical] = required.GetValueOrDefault(chemical) + inputQuantity * multiplier;
                required[next.Key] -= quantity * multiplier;
            }

            return required["ORE"];
        }

        long FindFuelQuantity() {
            const long target = 1000000000000L;
            return SearchHelpers.BinarySearchUpperBound(0, target, x => FindOreCount(x) <= target);
        }
    }
}
