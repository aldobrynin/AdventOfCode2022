// ReSharper disable once CheckNamespace
namespace Common;

public record SampleInput(string Input, string? PartOneAnswer = null, string? PartTwoAnswer = null) {
    public IEnumerable<string> MultilineInput() => Input.Split(Environment.NewLine);

    public static SampleInput ForInput(string input) => new(input);

    public SampleInput WithPartOneAnswer<T>(T answer) where T : notnull =>
        this with { PartOneAnswer = answer.ToString() };

    public SampleInput WithPartTwoAnswer<T>(T answer) where T : notnull =>
        this with { PartTwoAnswer = answer.ToString() };
}