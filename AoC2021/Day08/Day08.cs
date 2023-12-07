namespace AoC2021.Day08;

public class Day8
{
    public static void Solve(IEnumerable<string> input)
    {
        var array = input as string[] ?? input.ToArray();
        array.Select(line => line.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries)[1])
            .Select(line => line.Split(' '))
            .SelectMany(line => line.Where(s => s.Length is 2 or 3 or 4 or 7))
            .Count()
            .Dump("Part1: ");

        array.Sum(Decode).Dump("Part2: ");
    }

    private static long Decode(string line)
    {
        var split = line.Split('|');
        var signals = split[0].Split(' ', StringSplitOptions.RemoveEmptyEntries);
        var output = split[1].Split(' ', StringSplitOptions.RemoveEmptyEntries);

        var lookupByLength = signals.ToLookup(x => x.Length);

        var one = lookupByLength[2].Single();
        var seven = lookupByLength[3].Single();;
        var four = lookupByLength[4].Single();
        var eight = lookupByLength[7].Single();

        var top = seven.Except(one).Single();

        var nine = lookupByLength[6].Single(x => x.Except(four.Append(top)).Count() == 1);
        var bottom = nine.Except(four.Append(top)).Single();
        var leftBot = eight.Except(nine).Single();

        var three = lookupByLength[5].Single(x => x.Except(seven.Append(bottom)).Count() == 1);

        var middle = three.Except(seven.Append(bottom)).Single();

        var zero = signals.Single(x => x != eight && !eight.Except(x.Append(middle)).Any());

        var five = lookupByLength[5].Single(x => x != three && nine.Except(x).Count() == 1);

        var six = lookupByLength[6].Single(x => x != zero && x != nine);

        var rightBot = six.Except(five).Single();
        var two = lookupByLength[5].Single(x => x != three && !x.Append(rightBot).Except(three.Append(leftBot)).Any());
        var decoder = new Dictionary<string, int>
        {
            [zero] = 0,
            [one] = 1,
            [two] = 2,
            [three] = 3,
            [four] = 4,
            [five] = 5,
            [six] = 6,
            [seven] = 7,
            [eight] = 8,
            [nine] = 9
        };
        return output.Select(x => decoder.Single(kp => kp.Key.Order().SequenceEqual(x.Order())).Value)
            .Aggregate(0L, (cur, n) => cur * 10 + n);
    }
}