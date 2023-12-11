namespace AoC2020.Day22;

public partial class Day22 {
    public static void Solve(IEnumerable<string> input) {
        var players = input.SplitBy(string.IsNullOrWhiteSpace)
            .Select(x => x.Skip(1).Select(int.Parse).ToArray())
            .ToArray();

        PlayCombat(players[0], players[1]).Score.Part1();
        PlayCombatRecursive(players[0], players[1]).Score.Part2();
    }

    private static (int Winner, int Score) PlayCombatRecursive(
        IEnumerable<int> firstPlayer,
        IEnumerable<int> secondPlayer) {
        var firstDeck = new Queue<int>(firstPlayer);
        var secondDeck = new Queue<int>(secondPlayer);
        var visited = new HashSet<string>();

        while (firstDeck.Count > 0 && secondDeck.Count > 0) {
            var state = StringifyState();
            if (!visited.Add(state)) {
                return (1, CalculateScore(firstDeck));
            }

            var first = firstDeck.Dequeue();
            var second = secondDeck.Dequeue();
            int winner;
            if (firstDeck.Count >= first && secondDeck.Count >= second)
                (winner, _) = PlayCombatRecursive(firstDeck.Take(first), secondDeck.Take(second));
            else
                winner = first > second ? 1 : 2;
            var winnerDeck = winner == 1 ? firstDeck : secondDeck;
            winnerDeck.Enqueue(winner == 1 ? first : second);
            winnerDeck.Enqueue(winner == 1 ? second : first);
        }

        return (firstDeck.Count > 0 ? 1 : 2, CalculateScore(firstDeck.Count > 0 ? firstDeck : secondDeck));

        string StringifyState() => firstDeck.StringJoin() + "_" + secondDeck.StringJoin();
    }


    private static int CalculateScore(IEnumerable<int> deck) {
        return deck.Reverse().Select((num, ind) => num * (ind + 1)).Sum();
    }

    private static (int Winner, int Score) PlayCombat(IEnumerable<int> firstPlayer, IEnumerable<int> secondPlayer) {
        var firstDeck = new Queue<int>(firstPlayer);
        var secondDeck = new Queue<int>(secondPlayer);

        while (firstDeck.Count > 0 && secondDeck.Count > 0) {
            var first = firstDeck.Dequeue();
            var second = secondDeck.Dequeue();
            var winnerCard = Math.Max(first, second);
            var winner = winnerCard == first ? firstDeck : secondDeck;
            winner.Enqueue(winnerCard);
            winner.Enqueue(winnerCard == first ? second : first);
        }

        return (firstDeck.Count > 0 ? 1 : 2, CalculateScore(firstDeck.Count > 0 ? firstDeck : secondDeck));
    }
}