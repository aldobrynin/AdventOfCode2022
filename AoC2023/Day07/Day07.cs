namespace AoC2023.Day07;

public partial class Day07 {

    // ReSharper disable once NotAccessedPositionalProperty.Local
    private record Hand(string Cards) {
        private const string DefaultOrder = "23456789TJQKA";
        private const string WeakestJokerOrder = "J23456789TQKA";
        public readonly int HandType = GetHandType(Cards);
        public readonly long CardsValue = GetCardsScore(Cards, DefaultOrder);

        public readonly int HandTypeJokerAware = GetHandTypeJokerAware(Cards);
        public readonly long CardsValueJokerAware = GetCardsScore(Cards, WeakestJokerOrder);

        private static long GetCardsScore(string cards, string order) {
            return cards.Aggregate(1L, (acc, card) => 100 * acc + order.IndexOf(card));
        }

        private static int GetHandType(string hand) {
            var cardsFrequency = hand.CountFrequency().Select(x => x.Value).OrderDescending().ToArray();
            return cardsFrequency switch {
                [5] => 6,
                [4, _] => 5,
                [3, 2] => 4,
                [3, ..] => 3,
                [2, 2, _] => 2,
                [2, ..] => 1,
                _ => 0,
            };
        }

        private static int GetHandTypeJokerAware(string hand) {
            return hand.Contains('J')
                ? "23456789TQKA".Max(substituteRank => GetHandType(hand.Replace('J', substituteRank)))
                : GetHandType(hand);
        }
    }

    public static void Solve(IEnumerable<string> input) {
        var hands = input.Select(line => line.Split(' '))
            .Select(x => (Hand: new Hand(x[0]), Bid: x[1].ToLong()))
            .ToArray();

        hands
            .OrderBy(x => x.Hand.HandType)
            .ThenBy(x => x.Hand.CardsValue)
            .Select((x, i) => (i + 1) * x.Bid)
            .Sum()
            .Part1();

        hands
            .OrderBy(x => x.Hand.HandTypeJokerAware)
            .ThenBy(x => x.Hand.CardsValueJokerAware)
            .Select((x, i) => (i + 1) * x.Bid)
            .Sum()
            .Part2();
    }
}