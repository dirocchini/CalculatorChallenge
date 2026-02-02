namespace CalculatorChallenge.Application.Interfaces;

public interface IOperationStrategy
{
    string Key { get; }       // "add", "sub", "mul", "div"
    string Symbol { get; }    
    decimal Apply(IReadOnlyList<int> numbers);
}
