using System.Globalization;
using Common;

namespace Solution;

public class Day21
{
    public static void Solve(IEnumerable<string> input)
    {
        var monkeys = ParseTree(input);

        var root = monkeys["root"];
        root.Evaluate().Dump("Part1: ");

        monkeys["humn"].Value = new Equation(1, 0);
        var equation = root.Left!.Evaluate() - root.Right!.Evaluate();
        var result = -equation.B / equation.A;
        Math.Round(result).Dump("Part2: ");
    }

    private static Dictionary<string, TreeNode> ParseTree(IEnumerable<string> input)
    {
        var monkeys = input
            .Select(line =>
            {
                var parts = line.Split(new[] { ':', ' ' }, 2, StringSplitOptions.RemoveEmptyEntries);
                var equationTokens = parts[1].Split(' ');
                var value = equationTokens.Length == 1 ? long.Parse(equationTokens[0]) : 0;
                return new TreeNode(parts[0])
                {
                    Value = new Equation(0, value),
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

    private record TreeNode(string Name)
    {
        public Equation Value { get; set; } = null!;
        public string? Operator { get; init; }
        public TreeNode? Left { get; set; }
        public TreeNode? Right { get; set; }

        public Equation Evaluate()
        {
            return Operator switch
            {
                "+" => Left!.Evaluate() + Right!.Evaluate(),
                "-" => Left!.Evaluate() - Right!.Evaluate(),
                "*" => Left!.Evaluate() * Right!.Evaluate(),
                "/" => Left!.Evaluate() / Right!.Evaluate(),
                null => Value,
                _ => throw new ArgumentOutOfRangeException(nameof(Operator), Operator),
            };
        }
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