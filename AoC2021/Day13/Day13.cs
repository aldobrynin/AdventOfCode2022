using Spectre.Console;

namespace AoC2021.Day13;

public partial class Day13
{

    private record Instruction(string Coordinate, int Value)
    {
        public static Instruction Parse(string line)
        {
            var tokens = line.Replace("fold along ", String.Empty).Split('=');
            return new(Coordinate: tokens[0], Value: int.Parse(tokens[1]));
        }
    }

    public static void Solve(IEnumerable<string> input)
    {
        var s = input as string[] ?? input.ToArray();
        var map = s.TakeWhile(x => !string.IsNullOrEmpty(x))
            .Select(V.Parse)
            .ToHashSet();

        var instructions = s.Where(x => x.StartsWith("fold"))
            .Select(Instruction.Parse)
            .ToArray();

        Apply(map, instructions)
            .First().Count
            .Part1();

        Console.WriteLine("Part2: ");
        Print(Apply(map, instructions)
            .Last());
    }

    private static IEnumerable<IReadOnlySet<V>> Apply(IEnumerable<V> originalMap, IEnumerable<Instruction> instructions)
    {
        var map = new HashSet<V>(originalMap);
        foreach (var instruction in instructions)
        {
            var changes = instruction.Coordinate switch
            {
                "y" => map.Where(v => v.Y > instruction.Value)
                    .Select(v => (Old: v, New: v with { Y = 2 * instruction.Value - v.Y }))
                    .ToArray(),
                "x" => map.Where(v => v.X > instruction.Value)
                    .Select(v => (Old: v, New: v with { X = 2 * instruction.Value - v.X }))
                    .ToArray(),
                _ => throw new Exception(instruction.Coordinate),
            };

            map.ExceptWith(changes.Select(x => x.Old));
            map.UnionWith(changes.Select(x => x.New));

            yield return map;
        }
    }

    private static void Print(IReadOnlySet<V> points)
    {
        var minX = points.Min(x => x.X);
        var maxX = points.Max(x => x.X);
        var minY = points.Min(x => x.Y);
        var maxY = points.Max(x => x.Y);

        var canvas = new Canvas(maxX - minX + 1, maxY - minY + 1);
        for (var y = minY; y <= maxY; y++)
        for (var x = minX; x <= maxX; x++)
            canvas.SetPixel(x - minX, y - minY, points.Contains(new V(x, y)) ? Color.Black : Color.White);

        AnsiConsole.Write(canvas);

    }
}