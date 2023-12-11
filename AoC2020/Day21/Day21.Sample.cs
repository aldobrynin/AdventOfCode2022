namespace AoC2020.Day21;

public partial class Day21 {
    public static IEnumerable<SampleInput> GetSamples() {
        yield return SampleInput.ForInput("""
                                          mxmxvkd kfcds sqjhc nhms (contains dairy, fish)
                                          trh fvjkl sbzzf mxmxvkd (contains dairy)
                                          sqjhc fvjkl (contains soy)
                                          sqjhc mxmxvkd sbzzf (contains fish)
                                          """)
            .WithPartOneAnswer("5")
            .WithPartTwoAnswer("mxmxvkd,sqjhc,fvjkl");
    }
}