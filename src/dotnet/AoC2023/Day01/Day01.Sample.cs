namespace AoC2023.Day01;

public static partial class Day01 {
    public static IEnumerable<SampleInput> GetSamples() {
        yield return SampleInput.ForInput("""
                                          1abc2
                                          pqr3stu8vwx
                                          a1b2c3d4e5f
                                          treb7uchet
                                          """)
            .WithPartOneAnswer("142");
        
        yield return SampleInput.ForInput("""
                                          two1nine
                                          eightwothree
                                          abcone2threexyz
                                          xtwone3four
                                          4nineeightseven2
                                          zoneight234
                                          7pqrstsixteen
                                          """)
            .WithPartOneAnswer("209")
            .WithPartTwoAnswer("281");
    }
}