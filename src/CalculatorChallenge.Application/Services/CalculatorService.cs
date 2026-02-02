using CalculatorChallenge.Application.Interfaces;
using CalculatorChallenge.Application.Options;
using CalculatorChallenge.Domain;
using CalculatorChallenge.Domain.Exceptions;
using Microsoft.Extensions.Options;
using System.Data;

namespace CalculatorChallenge.Application.Services;

public sealed class CalculatorService(
    IParserService parserService, 
    IOptions<ParserOptions> options,
    IEnumerable<IOperationStrategy> strategies) : ICalculatorService
{
    private readonly ParserOptions _options = options.Value;

    private readonly Dictionary<string, IOperationStrategy> _strategies =
        strategies.ToDictionary(s => s.Key, StringComparer.OrdinalIgnoreCase);

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
            .Select(n => n <= _options.MaxValue ? n : 0)
            .ToList();

        if (!_options.AllowNegatives)
        {
            var negatives = numbers.Where(n => n < 0).ToArray();
            if (negatives.Length > 0)
                throw new NegativeNumbersNotAllowedException(negatives);
        }

        var op = ResolveOperation(_options.Operation);
        var result = op.Apply(numbers);

        var formula = string.Join(op.Symbol, numbers);

        return new CalculationResult
        {
            Result = result,
            Formula = formula
        };
    }

    private static int ParseInt(string? value) => int.TryParse(value, out var result) ? result : 0;

    private IOperationStrategy ResolveOperation(string? key)
    {
        key ??= "add";
        if (_strategies.TryGetValue(key, out var strategy))
            return strategy;

        throw new ArgumentException($"Unsupported operation: '{key}'. Use add|sub|mul|div.");
    }
}
