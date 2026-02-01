using CalculatorChallenge.Application.Interfaces;
using CalculatorChallenge.Application.Options;
using CalculatorChallenge.Domain;
using CalculatorChallenge.Domain.Exceptions;
using Microsoft.Extensions.Options;
using System.Data;

namespace CalculatorChallenge.Application.Services;

public sealed class CalculatorService(IParserService parserService, IOptions<ParserOptions> options) : ICalculatorService
{
    private readonly ParserOptions _options = options.Value;

    public CalculationResult Calculate(string expression)
    {

        if (string.IsNullOrWhiteSpace(expression))
            return new CalculationResult
            {
                Result = 0,
                Formula = "0"
            };

        var parsedValues = parserService.Parse(expression);

        var numbers = parsedValues
            .Select(ParseInt)
            .Select(n => n <= _options.MaxValue ? n : 0);

        if (!_options.AllowNegatives)
        {
            var negatives = numbers.Where(n => n < 0).ToArray();
            if (negatives.Length > 0)
                throw new NegativeNumbersNotAllowedException(negatives);
        }

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
