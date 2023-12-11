namespace AoC2022.Day25;

public partial class Day25
{
    public static void Solve(IEnumerable<string> input)
    {
        ToSnafu(input.Select(FromSnafu).Sum()).Part1();
    }

    private static long FromSnafu(string line) => line.Aggregate(0L,
        (current, c) => current * 5 + c switch { '-' => -1, '=' => -2, _ => c - '0' });

    private static string ToSnafu(long n) =>
        GetDigitsInBalancedBase(n, 5)
            .Select(c => c switch { -2 => '=', -1 => '-', _ => (char)(c + '0') })
            .Reverse()
            .StringJoin(string.Empty);

    private static IEnumerable<int> GetDigitsInBalancedBase(long n, int b)
    {
        while (n != 0)
        {
            var rem = (int)(n % b);
            n /= b;
            if (rem > b / 2)
            {
                rem -= b;
                n++;
            }

            yield return rem;
        }
    }
}