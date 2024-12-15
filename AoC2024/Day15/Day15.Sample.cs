namespace AoC2024.Day15;

public static partial class Day15 {
    public static IEnumerable<SampleInput> GetSamples() {
         yield return SampleInput.ForInput("""
                                           ########
                                           #..O.O.#
                                           ##@.O..#
                                           #...O..#
                                           #.#.O..#
                                           #...O..#
                                           #......#
                                           ########

                                           <^^>>>vv<v>>v<<
                                           """)
             .WithPartOneAnswer(2028);

          yield return SampleInput.ForInput("""
                                            ##########
                                            #..O..O.O#
                                            #......O.#
                                            #.OO..O.O#
                                            #..O@..O.#
                                            #O#..O...#
                                            #O..O..O.#
                                            #.OO.O.OO#
                                            #....O...#
                                            ##########

                                            <vv>^<v^>v>^vv^v>v<>v^v<v<^vv<<<^><<><>>v<vvv<>^v^>^<<<><<v<<<v^vv^v>^
                                            vvv<<^>^v^^><<>>><>^<<><^vv^^<>vvv<>><^^v>^>vv<>v<<<<v<^v>^<^^>>>^<v<v
                                            ><>vv>v^v^<>><>>>><^^>vv>v<^^^>>v^v^<^^>v^^>v^<^v>v<>>v^v^<v>v^^<^^vv<
                                            <<v<^>>^^^^>>>v^<>vvv^><v<<<>^^^vv^<vvv>^>v<^^^^v<>^>vvvv><>>v^<<^^^^^
                                            ^><^><>>><>^^<<^^v>>><^<v>^<vv>>v>>>^v><>^v><<<<v>>v<v<v>vvv>^<><<>^><
                                            ^>><>^v<><^vvv<^^<><v<<<<<><^v<<<><<<^^<v<^^^><^>>^<v^><<<^>>^v<v^v<v^
                                            >^>>^v>vv>^<<^v<>><<><<v<<v><>v<^vv<<<>^^v^>^^>>><<^v>>v^v><^^>>^<>vv^
                                            <><^^>^^^<><vvvvv^v<v<<>^v<v>v<<^><<><<><<<^^<<<^<<>><<><^^^>^^<>^>v<>
                                            ^^>vv<^v^v<vv>^<><v<^v>^^^>>>^^vvv^>vvv<>>>^<^>>>>>^<<^v>^vvv<>^<><<v>
                                            v^^>>><<^^<>>^v^<v^vv<>v^<<>^<^v^v><^<<<><<^<v><v<>vv>>v><v^<vv<>v^<<^
                                            """)
              .WithPartOneAnswer(10092)
              .WithPartTwoAnswer(9021);
    }
}
