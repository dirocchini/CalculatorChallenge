using CalculatorChallenge.Application.Interfaces;
using System.Data;

namespace CalculatorChallenge.Application.Services;

public sealed class CalculatorService(IParserService parserService) : ICalculatorService
{
    public int Calculate(string expression)
    {
        if (string.IsNullOrEmpty(expression))
            return 0;

        var values = parserService.Parse(expression);

        return values.Select(ParseInt).Sum();
    }
    private static int ParseInt(string? value) => int.TryParse(value, out var result) ? result : 0;
}
