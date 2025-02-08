namespace AoC2020.Day24;

public partial class Day24 {
    public static IEnumerable<SampleInput> GetSamples() {
        yield return SampleInput.ForInput("""
                                          sesenwnenenewseeswwswswwnenewsewsw
                                          neeenesenwnwwswnenewnwwsewnenwseswesw
                                          seswneswswsenwwnwse
                                          nwnwneseeswswnenewneswwnewseswneseene
                                          swweswneswnenwsewnwneneseenw
                                          eesenwseswswnenwswnwnwsewwnwsene
                                          sewnenenenesenwsewnenwwwse
                                          wenwwweseeeweswwwnwwe
                                          wsweesenenewnwwnwsenewsenwwsesesenwne
                                          neeswseenwwswnwswswnw
                                          nenwswwsewswnenenewsenwsenwnesesenew
                                          enewnwewneswsewnwswenweswnenwsenwsw
                                          sweneswneswneneenwnewenewwneswswnese
                                          swwesenesewenwneswnwwneseswwne
                                          enesenwswwswneneswsenwnewswseenwsese
                                          wnwnesenesenenwwnenwsewesewsesesew
                                          nenewswnwewswnenesenwnesewesw
                                          eneswnwswnwsenenwnwnwwseeswneewsenese
                                          neswnwewnwnwseenwseesewsenwsweewe
                                          wseweeenwnesenwwwswnew
                                          """)
            .WithPartOneAnswer("10")
            .WithPartTwoAnswer("2208");
    }
}