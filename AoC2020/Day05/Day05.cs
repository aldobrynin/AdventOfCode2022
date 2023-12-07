namespace AoC2020.Day05;

public class Day5
{
    public static void Solve(IEnumerable<string> input)
    {
        var seats = input
            .Select(Decode)
            .OrderBy(x => x)
            .ToArray();

        seats[^1].Dump("Part1: ");

        (seats
                .Where((seatId, ind) => ind < seats.Length - 1 && seatId + 1 != seats[ind + 1])
                .First() + 1)
            .Dump("Part2: ");
    }

    private static int Decode(string code)
    {
        return code.Aggregate(0, (cur, ch) => cur * 2 + (ch is 'B' or 'R' ? 1 : 0));
    }
}