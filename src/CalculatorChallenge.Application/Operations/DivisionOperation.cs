using CalculatorChallenge.Application.Interfaces;

namespace CalculatorChallenge.Application.Operations;

public sealed class DivisionOperation : IOperationStrategy
{
    public string Key => "div";
    public string Symbol => "/";

    public decimal Apply(IReadOnlyList<int> numbers)
    {
        if (numbers.Count == 0) return 0;

        decimal result = numbers[0];
        for (var i = 1; i < numbers.Count; i++)
        {
            if (numbers[i] == 0)
                throw new DivideByZeroException("Division by zero is not allowed.");
            result /= numbers[i]; 
        }
        return result;
    }
}

