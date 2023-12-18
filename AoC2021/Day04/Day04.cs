namespace AoC2021.Day04;

public partial class Day04 {

    private class BingoBoard {
        private readonly Map<int> _map;
        private Map<bool> _marked;
        private int? _winNumber;

        public BingoBoard(Map<int> lines) {
            _map = lines;
            _marked = InitMarked();
        }

        private Map<bool> InitMarked() => new(_map.SizeX, _map.SizeY);

        public void Reset() {
            _winNumber = null;
            _marked = InitMarked();
        }

        public void Mark(int number) {
            if (_winNumber.HasValue)
                throw new Exception("Already a winner!");
            foreach (var v in _map.Coordinates().Where(x => _map[x] == number)) _marked[v] = true;
            if (_marked.Rows().Any(row => row.All(s => s))
                || _marked.Columns().Any(col => col.All(e => e)))
                _winNumber = number;
        }

        public bool IsWinner() => _winNumber.HasValue;

        public int Score() {
            if (!_winNumber.HasValue)
                throw new Exception("Onl winner has score");
            return _marked.Coordinates()
                .Where(x => !_marked[x])
                .Select(x => _map[x])
                .Sum() * _winNumber.Value;
        }
    }

    public static void Solve(IEnumerable<string> input) {
        var array = input.ToArray();
        var numbers = array[0].Split(',').Select(int.Parse).ToArray();

        var boards = array.Skip(1)
            .Where(x => !string.IsNullOrEmpty(x))
            .Select(x => x.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray())
            .Chunk(5)
            .Select(x => new BingoBoard(Map.From(x)))
            .ToArray();

        Play(numbers, boards).First().Score().Part1();
        Play(numbers, boards).Last().Score().Part2();
    }

    private static IEnumerable<BingoBoard> Play(IEnumerable<int> numbers, BingoBoard[] boards) {
        foreach (var board in boards) board.Reset();
        foreach (var number in numbers) {
            foreach (var board in boards) board.Mark(number);
            foreach (var board in boards.Where(x => x.IsWinner()))
                yield return board;
            boards = boards.Where(x => x.IsWinner() == false).ToArray();
        }
    }
}