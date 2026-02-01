using CalculatorChallenge.Domain;

namespace CalculatorChallenge.Application.Interfaces;

public interface IParserService
{
    List<string> Parse(string? input);
}
