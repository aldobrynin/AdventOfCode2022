using System.Text.RegularExpressions;
using Common;

namespace AoC2020;

public class Day14 {
    public static void Solve(IEnumerable<string> input) {
        var inputArray = input.Select(Parse).ToArray();
        Part1(inputArray);
        Part2(inputArray);
    }

    private static (string Address, string Value) Parse(string line) {
        if (line.StartsWith("mask = ")) {
            return ("mask", line.Replace("mask = ", string.Empty));
        }

        var regex = new Regex(@"mem\[(?<addr>\d+)\] = (?<val>\d+)");
        var match = regex.Match(line);
        return (match.Groups["addr"].Value, match.Groups["val"].Value);
    }

    private static void Part1(IEnumerable<(string Address, string Value)> input) {
        var mask = string.Empty;
        var mem = new Dictionary<int, long>();
        foreach (var (address, value) in input) {
            if (address == "mask") mask = value;
            else mem[int.Parse(address)] = ApplyMask(int.Parse(value));
        }

        long ApplyMask(long value) {
            var binaryValue = Convert.ToString(value, 2).PadLeft(36, '0');
            return mask
                .Zip(binaryValue, (m, b) => m == 'X' ? b : m)
                .Aggregate(0L, (acc, current) => acc * 2 + (current - '0'));
        }

        mem.Sum(x => x.Value).Dump("Part1: ");
    }

    private static void Part2(IEnumerable<(string Address, string Value)> input) {
        var mask = string.Empty;
        var mem = new Dictionary<string, long>();
        foreach (var (address, value) in input) {
            if (address == "mask") mask = value;
            else {
                foreach (var memAddress in EnumerateAddresses(ApplyMask(long.Parse(address)))) {
                    mem[memAddress] = long.Parse(value);
                }
            }
        }

        string ApplyMask(long v) {
            var val = Convert.ToString(v, 2).PadLeft(36, '0');
            return new string(mask.Zip(val, (maskBit, valueBit) => maskBit == '0' ? valueBit : maskBit).ToArray());
        }

        IEnumerable<string> EnumerateAddresses(string s) {
            var segments = s.Split('X');
            var values = new[] { "0", "1" };

            return segments.Skip(1)
                .Aggregate(new List<string> { segments[0] },
                    (current, segment) => current.SelectMany(x => values.Select(v => x + v + segment)).ToList());
        }

        mem.Sum(x => x.Value).Dump("Part2: ");
    }
}