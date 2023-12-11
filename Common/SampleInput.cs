namespace Common;

public record SampleInput(string Input, string? PartOneAnswer = null, string? PartTwoAnswer = null) {
    public IEnumerable<string> MultilineInput() => Input.Split(Environment.NewLine);

    public static SampleInput ForInput(string input) => new(input);
    
    public SampleInput WithPartOneAnswer(string answer) => this with { PartOneAnswer = answer };
    public SampleInput WithPartTwoAnswer(string answer) => this with { PartTwoAnswer = answer };
}