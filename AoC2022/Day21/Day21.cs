using System.Globalization;

namespace Solution.Day21;

public class Day21
{
    public static void Solve(IEnumerable<string> input)
    {
        var lines = input as string[] ?? input.ToArray();
        var monkeys = ParseTree(lines);
        EvaluateNode(monkeys["root"]).Dump("Part1: ");

        monkeys = ParseTree(lines);
        var root = monkeys["root"];
        root.Operator = "-";
        monkeys["humn"].Value = new Equation(1, 0);
        var equation = EvaluateNode(root);
        Math.Round(-equation.B / equation.A).Dump("Part2: ");
    }

    private static Dictionary<string, TreeNode> ParseTree(IEnumerable<string> input)
    {
        var monkeys = input
            .Select(line =>
            {
                var parts = line.Split(new[] { ':', ' ' }, 2, StringSplitOptions.RemoveEmptyEntries);
                var equationTokens = parts[1].Split(' ');
                var value = equationTokens.Length == 1 ? (long?)long.Parse(equationTokens[0]) : null;
                return new TreeNode(parts[0])
                {
                    Value = value == null ? null : new Equation(0, value.Value),
                    Operator = equationTokens.Length > 1 ? equationTokens[1] : null,
                    Left = equationTokens.Length > 1 ? new TreeNode(equationTokens[0]) : null,
                    Right = equationTokens.Length > 1 ? new TreeNode(equationTokens[2]) : null,
                };
            })
            .ToDictionary(x => x.Name);
        foreach (var (_, monkey) in monkeys)
        {
            monkey.Left = monkey.Left == null ? null : monkeys[monkey.Left.Name];
            monkey.Right = monkey.Right == null ? null : monkeys[monkey.Right.Name];
        }

        return monkeys;
    }

    private static Equation EvaluateNode(TreeNode item)
    {
        var stack = new Stack<TreeNode>();
        stack.Push(item);
        while (stack.TryPeek(out var next) && item.Value == null)
        {
            if (next.Value != null)
            {
                stack.Pop();
                continue;
            }

            if (next.Left?.Value != null && next.Right?.Value != null)
            {
                next.Value = next.Operator switch
                {
                    "+" => next.Left.Value + next.Right.Value,
                    "-" => next.Left.Value - next.Right.Value,
                    "*" => next.Left.Value * next.Right.Value,
                    "/" => next.Left.Value / next.Right.Value,
                    _ => throw new Exception($"WTF: {next.Operator}"),
                };
                stack.Pop();
                continue;
            }

            if (next.Left is { Value: null }) stack.Push(next.Left);
            if (next.Right is { Value: null }) stack.Push(next.Right);
        }

        return item.Value ?? throw new Exception("couldn't solve it :(");
    }

    private record TreeNode(string Name)
    {
        public Equation? Value { get; set; }
        public string? Operator { get; set; }
        public TreeNode? Left { get; set; }
        public TreeNode? Right { get; set; }
    }

    // A*x^K + B
    private record Equation(decimal A, decimal B)
    {
        public static Equation operator +(Equation left, Equation right) =>
            new(left.A + right.A, left.B + right.B);

        public static Equation operator -(Equation left, Equation right) =>
            new(left.A - right.A, left.B - right.B);

        // (A1*x+B1)*(A2*x+B2) => 
        public static Equation operator *(Equation left, Equation right)
        {
            if (left.A != 0 && right.A != 0)
                throw new Exception("WTF");
            return left.A != 0
                ? new Equation(left.A * right.B, left.B * right.B)
                : new Equation(right.A * left.B, left.B * right.B);
        }

        // (A1*x+B1)/(A2*x+B2)=> 
        public static Equation operator /(Equation left, Equation right)
        {
            if (left.A != 0 && right.A != 0)
                throw new Exception("WTF");
            if (left.A != 0)
                return new Equation(left.A / right.B, left.B / right.B);
            return new Equation(0, left.B / right.B);
        }

        public override string ToString()
        {
            return A == 0 ? B.ToString(CultureInfo.InvariantCulture) : $"{A}*x+{B}";
        }
    }
}