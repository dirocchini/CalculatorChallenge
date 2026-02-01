namespace CalculatorChallenge.Application.Options;

public sealed class ParserOptions
{
    public string AlternateDelimiter { get; set; } = @"\n";
    public int MaxValue { get; set; } = 1000;
    public bool AllowNegatives { get; set; } = false;
}
