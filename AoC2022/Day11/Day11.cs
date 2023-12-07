using System.Linq.Expressions;

namespace Solution.Day11;

public record Monkey(int Number,
    Queue<long> Items,
    Func<long, long> Operation,
    int Divider,
    int IfTrue,
    int IfFalse)
{
    public long InspectionCount { get; set; }

    public override string ToString()
    {
        return $"Monkey: {Number}, Inspections: {InspectionCount}, Items: {string.Join(",", Items)}";
    }

    public static Monkey Parse(string[] lines)
    {
        var divider = int.Parse(lines[3].Split(' ', StringSplitOptions.RemoveEmptyEntries).Last());
        var ifTrue = int.Parse(lines[4].Split(' ', StringSplitOptions.RemoveEmptyEntries).Last());
        var ifFalse = int.Parse(lines[5].Split(' ', StringSplitOptions.RemoveEmptyEntries).Last());
        var operationExpr = ParseExpression(lines[2].Replace("  Operation: new = ", string.Empty));
        var number = int.Parse(lines[0].Replace("Monkey", string.Empty).Replace(":", string.Empty));
        return new Monkey(
            number,
            new Queue<long>(lines[1].Replace("Starting items:", String.Empty)
                .Replace(" ", string.Empty)
                .Split(',').Select(long.Parse)),
            operationExpr,
            divider,
            ifTrue,
            ifFalse
        );

        static Func<long, long> ParseExpression(string input)
        {
            var parameter = Expression.Parameter(typeof(long), "old");

            Expression GetParameter(string value)
            {
                if (value == "old")
                    return parameter;
                return Expression.Constant(long.Parse(value));
            }

            var tokens = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            Func<Expression, Expression, BinaryExpression> operationExpression = tokens[1] switch
            {
                "*" => Expression.MultiplyChecked,
                "+" => Expression.AddChecked,
                _ => throw new ArgumentOutOfRangeException(nameof(input), tokens[1]),
            };
            var expression = operationExpression(GetParameter(tokens[0]), GetParameter(tokens[2]));
            return Expression.Lambda<Func<long, long>>(expression, parameter).Compile();
        }
    }
}

public class Day11
{
    public static void Solve(IEnumerable<string> input)
    {
        var monkeys = input
            .Chunk(7)
            .Select(Monkey.Parse)
            .ToArray();
        GetMonkeyBusinessAfterRounds(CloneMonkeys(), roundsCount: 20, worryDivider: 3)
            .Dump("Part1: ");
        GetMonkeyBusinessAfterRounds(CloneMonkeys(), roundsCount: 10000, worryDivider: 1)
            .Dump("Part2: ");

        Monkey[] CloneMonkeys()
        {
            return monkeys.Select(x => x with { InspectionCount = 0, Items = new Queue<long>(x.Items) }).ToArray();
        }
    }

    private static long GetMonkeyBusinessAfterRounds(Monkey[] monkeys, int roundsCount, int worryDivider)
    {
        var commonDivider = monkeys.Select(x => x.Divider).Product();
        for (var round = 0; round < roundsCount; round++)
        {
            foreach (var monkey in monkeys)
            {
                while (monkey.Items.TryDequeue(out var item))
                {
                    monkey.InspectionCount++;
                    var nextWorry = monkey.Operation(item) / worryDivider;
                    var nextMonkeyIndex = nextWorry % monkey.Divider == 0 ? monkey.IfTrue : monkey.IfFalse;
                    monkeys[nextMonkeyIndex].Items.Enqueue(nextWorry % commonDivider);
                }
            }
        }

        return monkeys
            .Select(x => x.InspectionCount)
            .OrderDescending()
            .Take(2)
            .Product();
    }
}