namespace AoC2022.Day01;

public partial class Day01
{
    public static void Solve(IEnumerable<string> input)
    {
        var elvesCalories = input.StringJoin("\n")
            .Split("\n\n")
            .Select(elfLines => elfLines.Split('\n').Sum(int.Parse))
            .OrderDescending()
            .ToArray();
        
        elvesCalories.First().Part1();
        elvesCalories.Take(3).Sum().Part2();
    }
}