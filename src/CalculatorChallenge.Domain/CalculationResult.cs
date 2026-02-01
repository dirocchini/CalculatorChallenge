namespace CalculatorChallenge.Domain;

public sealed class CalculationResult
{
    public int Result { get; init; }
    public string Formula { get; init; } = string.Empty;
}
