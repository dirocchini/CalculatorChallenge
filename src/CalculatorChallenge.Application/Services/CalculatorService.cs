using CalculatorChallenge.Application.Interfaces;
using CalculatorChallenge.Domain.Exceptions;
using System.Data;

namespace CalculatorChallenge.Application.Services;

public sealed class CalculatorService : ICalculatorService
{
    public int Calculate(string expression)
    {
        if (string.IsNullOrEmpty(expression))
            return 0;

        var values = expression.Split(',', StringSplitOptions.RemoveEmptyEntries);
                               
        return values.Select(ParseInt).Sum();
    }
    private static int ParseInt(string? value) => int.TryParse(value, out var result) ? result : 0;
}
