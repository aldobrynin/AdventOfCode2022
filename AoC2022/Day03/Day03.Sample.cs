namespace AoC2022.Day03;

public partial class Day03 {
    public static IEnumerable<SampleInput> GetSamples() {
        yield return SampleInput.ForInput("""
                                          vJrwpWtwJgWrhcsFMMfFFhFp
                                          jqHRNqRjqzjGDLGLrsFMfFZSrLrFZsSL
                                          PmmdzqPrVvPwwTWBwg
                                          wMqvLMZHhHMvwLHjbvcjnnSBnvTQFn
                                          ttgJtRGJQctTZtZT
                                          CrZsJsPPZsGzwwsLwLmpwMDw
                                          """)
            .WithPartOneAnswer("157")
            .WithPartTwoAnswer("70");
    }
}