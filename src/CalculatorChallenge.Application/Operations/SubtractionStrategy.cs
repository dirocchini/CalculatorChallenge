using CalculatorChallenge.Application.Interfaces;

namespace CalculatorChallenge.Application.Operations;

public sealed class SubtractionStrategy : IOperationStrategy
{
    public string Key => "sub";
    public string Symbol => "-";

    public decimal Apply(IReadOnlyList<int> numbers)
    {
        if (numbers.Count == 0) return 0;
        var result = numbers[0];
        for (var i = 1; i < numbers.Count; i++)
            result -= numbers[i];
        return result;
    }
}
