using CalculatorChallenge.Domain;

namespace CalculatorChallenge.Application.Interfaces;

public interface ICalculatorService
{
    CalculationResult Calculate(string expression);
}
