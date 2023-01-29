using System.Text.RegularExpressions;
using Common;

namespace AoC2020;

public class Day4
{
    public static void Solve(IEnumerable<string> input)
    {
        var passports = input.StringJoin(Environment.NewLine)
            .Split(Environment.NewLine + Environment.NewLine)
            .Select(l => l.Split(new[] { ' ', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(kv => kv.Split(':', 2))
                .ToDictionary(x => x[0], x => x[1])
            )
            .ToArray();

        passports.Count(p => RequiredFields.All(p.ContainsKey))
            .Dump("Part1: ");

        passports.Count(IsValid)
            .Dump("Part2: ");
    }

    private static bool IsValid(Dictionary<string, string> passport)
    {
        if (!passport.TryGetValue("byr", out var byr)
            || !int.TryParse(byr, out var byrValue)
            || byrValue is < 1920 or > 2002)
            return false;

        if (!passport.TryGetValue("iyr", out var iyr)
            || !int.TryParse(iyr, out var iyrValue)
            || iyrValue is < 2010 or > 2020)
            return false;

        if (!passport.TryGetValue("eyr", out var eyr)
            || !int.TryParse(eyr, out var eyrValue)
            || eyrValue is < 2020 or > 2030)
            return false;

        if (!passport.TryGetValue("hgt", out var hgt))
            return false;

        var match = HeightRegex.Match(hgt);
        if (!match.Success)
            return false;
        var value = int.Parse(match.Groups["Value"].Value);
        switch (match.Groups["Unit"].Value)
        {
            case "cm" when value is < 150 or > 193:
            case "in" when value is < 59 or > 76:
                return false;
        }

        if (!passport.TryGetValue("hcl", out var hcl) || !HexColorRegex.IsMatch(hcl))
            return false;

        if (!passport.TryGetValue("ecl", out var ecl) || !ValidEyeColors.Contains(ecl))
            return false;

        if (!passport.TryGetValue("pid", out var pid) || !PassportNoRegex.IsMatch(pid))
            return false;

        return true;
    }

    private static readonly string[] ValidEyeColors =
    {
        "amb", "blu", "brn", "gry", "grn", "hzl", "oth"
    };

    private static readonly string[] RequiredFields =
    {
        "byr", // (Birth Year)
        "iyr", // (Issue Year)
        "eyr", // (Expiration Year)
        "hgt", // (Height)
        "hcl", // (Hair Color)
        "ecl", // (Eye Color)
        "pid", // (Passport ID)
        // "cid", // (Country ID)
    };

    private static readonly Regex HexColorRegex = new("^#[a-f0-9]{6}$", RegexOptions.Compiled);
    private static readonly Regex PassportNoRegex = new("^\\d{9}$", RegexOptions.Compiled);
    private static readonly Regex HeightRegex = new("(?<Value>\\d+)(?<Unit>cm|in)", RegexOptions.Compiled);
}