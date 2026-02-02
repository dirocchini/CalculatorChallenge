using CalculatorChallenge.Application.Interfaces;

namespace CalculatorChallenge.Application.Operations;

public sealed class MultiplicationOperation : IOperationStrategy
{
    public string Key => "mul";
    public string Symbol => "*";

    public decimal Apply(IReadOnlyList<int> numbers)
    {
        if (numbers.Count == 0) return 0;
        var result = 1;
        foreach (var n in numbers)
            result *= n;
        return result;
    }
}
