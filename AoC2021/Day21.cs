using System.Numerics;
using Common;

namespace AoC2021;

public class Day21
{
    private record Player(int Position, long Score = 0);

    public static void Solve(IEnumerable<string> input)
    {
        var players = input
            .Select(line => line.Split(' ').Last())
            .Select(int.Parse)
            .Select(pos => new Player(pos))
            .ToArray();

        SimulatePart1(players).Dump("Part1: ");

        SimulatePart2(players).Max().Dump("Part2: ");
    }

    private static long SimulatePart1(Player[] initialState)
    {
        var dice = 1;
        var players = initialState.Select(x => new Player(x.Position, x.Score)).ToArray();
        while (true)
        {
            for (var index = 0; index < players.Length; index++)
            {
                var player = players[index];
                var move = Enumerable.Range(0, 3).Sum(_ => (dice++ - 1) % 100 + 1);

                var nextPosition = (player.Position + move - 1) % 10 + 1;
                players[index] = new Player(nextPosition, player.Score + nextPosition);

                if (players[index].Score >= 1000)
                    return players.Min(x => x.Score) * (dice - 1);
            }
        }
    }

    private static long[] SimulatePart2(Player[] initialState)
    {
        var cache = new Dictionary<CacheKey, Vector<long>>();
        var players = initialState.Select(x => new Player(x.Position, x.Score)).ToArray();
        var wins = GetWins(players, 0, cache);
        var res = new long[players.Length];
        wins.CopyTo(res);
        return res;
    }

    private static Vector<long> GetWins(Player[] players, int playerIndex, IDictionary<CacheKey, Vector<long>> outcomes)
    {
        if (players.Any(x => x.Score >= 21))
            return new Vector<long>(players.Select(x => x.Score >= 21 ? 1L : 0).ToArray());
        var result = new Vector<long>(Enumerable.Repeat(0L, players.Length).ToArray());
        var player = players[playerIndex];

        var diceRoll = new[] { 1, 2, 3 };
        foreach (var newPosition in diceRoll
                     .SelectMany(d1 => diceRoll
                         .SelectMany(d2 => diceRoll.Select(d3 => d1 + d2 + d3)))
                     .Select(move => (player.Position + move - 1) % 10 + 1)
                )
        {
            var newPlayer = new Player(newPosition, Score: player.Score + newPosition);
            var nextPlayers = players.Select((p, index) => index == playerIndex ? newPlayer : p).ToArray();
            var nextPlayerIndex = (playerIndex + 1) % players.Length;

            var cacheKey = new CacheKey(nextPlayers, nextPlayerIndex);
            if (!outcomes.TryGetValue(cacheKey, out var cached))
                cached = outcomes[cacheKey] = GetWins(nextPlayers, nextPlayerIndex, outcomes);
            result += cached;
        }

        return result;
    }

    private record CacheKey(Player[] Players, int PlayerIndex)
    {
        public virtual bool Equals(CacheKey? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Players.SequenceEqual(other.Players) && PlayerIndex == other.PlayerIndex;
        }

        public override string ToString()
        {
            return $"Index={PlayerIndex}; {Players.StringJoin()}";
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Players.Aggregate(17, (cur, p) => cur * 23 + p.GetHashCode()), PlayerIndex);
        }
    }
}