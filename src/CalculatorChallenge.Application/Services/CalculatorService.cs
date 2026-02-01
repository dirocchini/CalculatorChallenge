using CalculatorChallenge.Application.Interfaces;
using CalculatorChallenge.Domain;
using CalculatorChallenge.Domain.Exceptions;
using System.Data;

namespace CalculatorChallenge.Application.Services;

public sealed class CalculatorService(IParserService parserService) : ICalculatorService
{
    public CalculationResult Calculate(string expression)
    {
        if (string.IsNullOrEmpty(expression))
            return new CalculationResult
            {
                Result = 0,
                Formula = "0"
            };

        var parsedValues = parserService.Parse(expression);

        var numbers = parsedValues
            .Select(ParseInt)
            .Select(n => n <= 1000 ? n : 0);
        
        var negatives = numbers.Where(n => n < 0).ToArray();
        if (negatives.Length > 0)
            throw new NegativeNumbersNotAllowedException(negatives);

        var formula = string.Join("+", numbers);
        var result = numbers.Sum();

        return new CalculationResult
        {
            Result = result,
            Formula = formula
        };
    }
    private static int ParseInt(string? value) => int.TryParse(value, out var result) ? result : 0;
}
