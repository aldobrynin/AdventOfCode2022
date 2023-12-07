namespace AoC2020.Day18;

public class Day18 {
    public static void Solve(IEnumerable<string> input) {
        var operationsPriority = new Dictionary<string, int> {
            ["+"] = 0,
            ["*"] = 0,
        };
        input.Select(tokens => ToRPN(tokens, operationsPriority))
            .Select(EvalRPN)
            .Sum()
            .Dump("Part1: ");

        var advancedMathPriority = new Dictionary<string, int> {
            ["+"] = 0,
            ["*"] = 1,
        };
        input.Select(tokens => ToRPN(tokens, advancedMathPriority))
            .Select(EvalRPN)
            .Sum()
            .Dump("Part2: ");
    }

    private static string[] NormalizeAndSplitToTokens(string expr) {
        return expr.Replace("(", "( ").Replace(")", " )").Split(' ');
    }

    private static long EvalRPN(IEnumerable<string> expr) {
        var stack = new Stack<long>();
        foreach (var token in expr) {
            if (token == "+") stack.Push(stack.Pop() + stack.Pop());
            else if (token == "*") stack.Push(stack.Pop() * stack.Pop());
            else stack.Push(long.Parse(token));
        }

        return stack.Pop();
    }

    private static IEnumerable<string> ToRPN(string input, IReadOnlyDictionary<string, int> operationsPriority) {
        var stack = new Stack<string>();
        foreach (var token in NormalizeAndSplitToTokens(input)) {
            if (token == "(") stack.Push(token);
            else if (token == ")") {
                while (stack.TryPop(out var prevToken) && prevToken != "(")
                    yield return prevToken;
            }
            else if (operationsPriority.TryGetValue(token, out var currentPriority)) {
                while (stack.TryPeek(out var nextOperation) &&
                       operationsPriority.GetValueOrDefault(nextOperation, int.MaxValue) <= currentPriority)
                    yield return stack.Pop();
                stack.Push(token);
            }
            else yield return token;
        }

        while (stack.TryPop(out var el))
            yield return el;
    }
}