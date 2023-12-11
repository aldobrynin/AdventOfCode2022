namespace AoC2022.Day10;

public partial class Day10
{
    public static void Solve(IEnumerable<string> input)
    {
        var lines = input.Select(x => x.Split(' '));
        var values = new List<int>();
        var signalStrength = 1;
        foreach (var line in lines)
        {
            values.Add(signalStrength);
            if (line[0] == "addx")
            {
                values.Add(signalStrength);
                signalStrength += int.Parse(line[1]);
            }
        }

        values
            .Indices()
            .Where(i => (i + 1) % 40 == 20)
            .Sum(i => values[i] * (i + 1))
            .Part1();

        const int rowLength = 40;
        var crtLines = values.Chunk(rowLength)
            .Select(row => string.Join(string.Empty, row.Select((x, i) => Math.Abs(i - x) <= 1 ? "##" : "  ")));
        Console.WriteLine("Part2:\n");
        foreach (var line in crtLines) Console.WriteLine(line);
    }
}