using CalculatorChallenge.Application.Interfaces;

namespace CalculatorChallenge.Application.Operations;

public sealed class AdditionOperation : IOperationStrategy
{
    public string Key => "add";
    public string Symbol => "+";

    public decimal Apply(IReadOnlyList<int> numbers)
        => numbers.Sum();
}
