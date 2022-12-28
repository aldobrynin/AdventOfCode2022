using Common;

namespace Solution;

public class Day6
{
    public static void Solve(IEnumerable<string> input)
    {
        var signal = input.Single();
        FindIndex(signal, packetLength: 4).Dump("Part1: ");
        FindIndex(signal, packetLength: 14).Dump("Part2: ");
    }

    private static int FindIndex(string s, int packetLength)
    {
        return s.Indices()
                   .Where(x => x + packetLength < s.Length)
                   .First(i => s.Substring(i, packetLength).Distinct().Count() == packetLength)
               + packetLength;
    }
}