using Common;

namespace Solution;

public class Day25
{
    public static void Solve(IEnumerable<string> input)
    {
        ToSnafu(input.Select(FromSnafu).Sum().Dump("Sum: ")).Dump("Part1: ");
    }

    private static long FromSnafu(string line) => line.Aggregate(0L, (current, c) => current * 5 + c switch { '-' => -1, '=' => -2, _ => c - '0' });

    private static string ToSnafu(long n) =>
        GetDigitsInBase(n)
            .Select(c => c switch { -2 => '=', -1 => '-', _ => (char)(c + '0') })
            .Reverse()
            .StringJoin(string.Empty);

    private static IEnumerable<int> GetDigitsInBase(long n)
    {
        while (n != 0)
        {
            var rem = (int)(n % 5);
            n /= 5;
            if (rem > 5 / 2)
            {
                rem -= 5;
                n++;
            }
            yield return rem;
        }
    }
}