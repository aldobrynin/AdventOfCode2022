namespace Common.AoC;

public static class AoCContext {
    public static int Year { get; set; }
    public static int Day { get; set; }
    public static bool IsSample { get; set; }

    public static (string? Part1, string? Part2) Answers { get; set; }
}