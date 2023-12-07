namespace Solution.Day01;

public class Day1
{
    public static void Solve(IEnumerable<string> input)
    {
        var elvesCalories = input.StringJoin("\n")
            .Split("\n\n")
            .Select(elfLines => elfLines.Split('\n').Sum(int.Parse))
            .OrderDescending()
            .ToArray();
        
        elvesCalories.First().Dump("Part1: ");
        elvesCalories.Take(3).Sum().Dump("Part2: ");
    }
}