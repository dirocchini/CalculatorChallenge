using CalculatorChallenge.Application.Interfaces;
using CalculatorChallenge.Domain.Exceptions;
using System.Data;

namespace CalculatorChallenge.Application.Services;

public sealed class CalculatorService(IParserService parserService) : ICalculatorService
{
    public int Calculate(string expression)
    {
        if (string.IsNullOrEmpty(expression))
            return 0;

        var parsedValues = parserService.Parse(expression);

        var numbers = parsedValues.Select(ParseInt);
        
        var negatives = numbers.Where(n => n < 0).ToArray();
        if (negatives.Length > 0)
            throw new NegativeNumbersNotAllowedException(negatives);

        return numbers.Sum();
    }
    private static int ParseInt(string? value) => int.TryParse(value, out var result) ? result : 0;
}
