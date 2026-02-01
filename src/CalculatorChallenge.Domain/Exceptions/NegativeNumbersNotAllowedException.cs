namespace CalculatorChallenge.Domain.Exceptions;

public sealed class NegativeNumbersNotAllowedException : Exception
{
    public IReadOnlyList<int> Negatives { get; }

    public NegativeNumbersNotAllowedException(IEnumerable<int> negatives)
        : base($"Negatives not allowed: {string.Join(",", negatives)}")
    {
        Negatives = negatives.ToArray();
    }
}
