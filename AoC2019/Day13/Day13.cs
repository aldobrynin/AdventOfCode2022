namespace AoC2019.Day13;

public static partial class Day13 {
    public static void Solve(IEnumerable<string> input) {
        var program = input.Single().ToLongArray();

        new IntCodeComputer(program).ReadGameOutput().Count(x => x.TileId == 2).Part1();

        var freePlayProgram = program.ToArray();
        freePlayProgram[0] = 2;
        var ballX = 0L;
        var paddleX = 0L;

        new IntCodeComputer(freePlayProgram) { OnInput = () => Math.Sign(ballX - paddleX) }
            .ReadGameOutput()
            .Pipe(s => {
                if (s.TileId == 3) paddleX = s.X;
                else if (s.TileId == 4) ballX = s.X;
            })
            .Last(s => s is { X: -1, Y: 0 })
            .TileId.Part2();
    }

    private static IEnumerable<(long X, long Y, long TileId)> ReadGameOutput(this IntCodeComputer computer) {
        return computer.ReadAllOutputs().Chunk(3).Select(block => (block[0], block[1], block[2]));
    }
}