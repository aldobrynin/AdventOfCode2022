namespace AoC2020.Day20;

public partial class Day20 {
    public readonly record struct Tile(long Id, Map<char> Map) {
        public static Tile Parse(IReadOnlyList<string> input) {
            return new Tile(
                long.Parse(input[0].Split(new[] { ' ', ':' }, StringSplitOptions.RemoveEmptyEntries)[1]),
                Common.Map.From(input.Skip(1).Select(s => s.ToCharArray()).ToArray())
            );
        }

        public int Size => Map.SizeX;


        public IEnumerable<Tile> GetAllRotations() => Transformations.Select(ApplyTransform);

        private Tile ApplyTransform(Func<Map<char>, Map<char>> transform) => this with { Map = transform(Map) };

        public IReadOnlyList<string> GetBorders() => Map.Borders().Select(s => new string(s)).ToArray();
    }

    public static void Solve(IEnumerable<string> input) {
        var tiles = input.SplitBy(string.IsNullOrWhiteSpace)
            .Select(Tile.Parse)
            .ToDictionary(x => x.Id);

        var gridSize = (int)Math.Sqrt(tiles.Count);
        var grid = new Map<long>(gridSize, gridSize);

        var borders = tiles.SelectMany(t => t.Value.GetBorders().Select(b => (t.Key, b)))
            .GroupBy(x => NormalizeBorder(x.b))
            .ToDictionary(x => x.Key, x => x.Select(s => s.Key).ToArray());
        var counts = borders.ToDictionary(x => x.Key, x => x.Value.Length);

        var neighborsCountPerTile = tiles
            .Select(tile => (tile.Key, cnt: tile.Value.GetBorders().Count(b => counts[NormalizeBorder(b)] > 1)));
        var cornerIds = neighborsCountPerTile.Where(x => x.cnt == 2).Select(x => x.Key).ToArray();
        cornerIds.Product().Part1();

        for (var row = 0; row < gridSize; row++) {
            for (var col = 0; col < gridSize; col++) {
                if (row == 0 && col == 0) {
                    RotateUntilAndPut(cornerIds.First(), row, col, IsValidTopLeftCornerTile);
                }
                else if (col != 0) {
                    var prevTile = tiles[grid[row, col - 1]];
                    var rightBorder = prevTile.GetBorders()[2];
                    var nextTile = OtherTilesWithBorder(prevTile.Id, rightBorder).Single();
                    RotateUntilAndPut(nextTile, row, col,
                        candidate => IsValidRightNeighborTile(prevTile, candidate));
                }
                else {
                    var prevTile = tiles[grid[row - 1, col]];
                    var bottomBorder = prevTile.GetBorders()[3];
                    var nextTile = OtherTilesWithBorder(prevTile.Id, bottomBorder).Single();
                    RotateUntilAndPut(nextTile, row, col,
                        candidate => IsValidBottomNeighborTile(prevTile, candidate));
                }
            }
        }

        var tileSize = tiles.First().Value.Size;
        var seaSize = gridSize * (tileSize - 2);
        var seaMap = new Map<char>(seaSize, seaSize);
        foreach (var gridCoord in grid.Coordinates()) {
            var tile = tiles[grid[gridCoord]];
            foreach (var tileCoord in tile.Map.Coordinates()) {
                if (tileCoord.X == 0 || tileCoord.X == tileSize - 1 ||
                    tileCoord.Y == 0 || tileCoord.Y == tileSize - 1)
                    continue;
                var seaRow = gridCoord.Y * (tileSize - 2) + tileCoord.Y - 1;
                var seaCol = gridCoord.X * (tileSize - 2) + tileCoord.X - 1;
                seaMap[seaRow, seaCol] = tile.Map[tileCoord];
            }
        }

        var monster = new[] {
                "                  # ",
                "#    ##    ##    ###",
                " #  #  #  #  #  #   ",
            }
            .Select(x => x.ToCharArray())
            .ToArray();
        var cellsInMonster = monster.FindAll('#').Count();
        var totalCells = seaMap.FindAll('#').Count();
        var monstersCount = FindMonster(seaMap, monster).Length;
        var safeCells = totalCells - monstersCount * cellsInMonster;
        safeCells.Part2();


        bool IsValidTopLeftCornerTile(Tile tile) {
            return tile.GetBorders().Take(2).All(border => TilesWithBorder(border).Count() == 1);
        }

        bool IsValidRightNeighborTile(Tile leftNeighbor, Tile rightNeighbor) {
            return leftNeighbor.GetBorders()[2] == rightNeighbor.GetBorders()[0];
        }

        bool IsValidBottomNeighborTile(Tile topNeighbor, Tile bottomNeighbor) {
            return topNeighbor.GetBorders()[3] == bottomNeighbor.GetBorders()[1];
        }

        IEnumerable<long> TilesWithBorder(string border) {
            return borders[NormalizeBorder(border)];
        }

        IEnumerable<long> OtherTilesWithBorder(long tileId, string border) {
            return TilesWithBorder(border).Where(x => x != tileId);
        }

        void RotateUntilAndPut(long tileId, int row, int col, Func<Tile, bool> isValidTransform) {
            foreach (var rotatedTile in tiles[tileId].GetAllRotations()) {
                if (!isValidTransform(rotatedTile)) continue;
                grid[row, col] = rotatedTile.Id;
                tiles[tileId] = rotatedTile;
                return;
            }

            throw new Exception($"Couldn't find valid rotation for tile {tileId}");
        }
    }

    private static V[] FindMonster(Map<char> map, char[][] monster) {
        var monsters = new List<V>();
        foreach (var transformedMap in Transformations.Select(t => t(map))) {
            foreach (var start in transformedMap.Coordinates()) {
                if (IsMonster(start, transformedMap)) monsters.Add(start);
            }

            if (monsters.Count > 0)
                break;
        }

        return monsters.ToArray();

        bool IsMonster(V start, Map<char> sea) {
            return
                monster.Coordinates()
                    .Where(v => monster.Get(v) != ' ')
                    .Select(mCoordinate => (monsterCoordinate: mCoordinate, seaCoordinate: start + mCoordinate))
                    .All(x => x.seaCoordinate.IsInRange(sea) &&
                              monster.Get(x.monsterCoordinate) == sea[x.seaCoordinate]);
        }
    }

    private static readonly Func<Map<char>, Map<char>>[] Transformations = {
        m => m,
        m => m.Rotate90(),
        m => m.Rotate180(),
        m => m.Rotate270(),
        m => m.Transpose(),
        m => m.Transpose().Rotate90(),
        m => m.Transpose().Rotate180(),
        m => m.Transpose().Rotate270(),
    };

    private static string NormalizeBorder(string s) {
        var reversed = new string(s.Reverse().ToArray());
        return string.CompareOrdinal(s, reversed) > 0 ? s : reversed;
    }
}