namespace AoC2022.Day02;

public partial class Day02
{
    public static void Solve(IEnumerable<string> input)
    {
        var lines = input.ToArray();
        lines.Sum(line => GetScore((Shape)(line[0] - 'A'), (Shape)line[2] - 'X'))
            .Part1();
        
        lines.Sum(line =>
        {
            var elf = (Shape) line[0] - 'A';
            var outcome = (Outcome)(line[2] - 'X');
            return GetScore(Guess(elf, outcome), outcome);
        }).Part2();
    }

    private static int GetScore(Shape me, Outcome outcome) => (int)me + 1 + (int)outcome * 3;
    private static int GetScore(Shape elf, Shape me) => (int)me + 1 + (me - elf + 4) % 3 * 3;

    private static Shape Guess(Shape opp, Outcome desiredOutcome)
    {
        return (opp, desiredOutcome) switch
        {
            (_, Outcome.Draw) => opp,
            (Shape.Paper, Outcome.Win) => Shape.Scissors,
            (Shape.Paper, Outcome.Lose) => Shape.Rock,
            (Shape.Rock, Outcome.Win) => Shape.Paper,
            (Shape.Rock, Outcome.Lose) => Shape.Scissors,
            (Shape.Scissors, Outcome.Win) => Shape.Rock,
            (Shape.Scissors, Outcome.Lose) => Shape.Paper,
            _ => throw new ArgumentOutOfRangeException()
        };
    }
    
    internal enum Shape
    {
        Rock = 0,
        Paper = 1,
        Scissors = 2,
    }
    
    internal enum Outcome
    {
        Lose = 0,
        Draw = 1,
        Win = 2,
    }
}