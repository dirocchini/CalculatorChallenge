namespace CalculatorChallenge.Domain;

public sealed class CalculationResult
{
    public decimal Result { get; init; }
    public string Formula { get; init; } = string.Empty;
}
