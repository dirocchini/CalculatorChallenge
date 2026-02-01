namespace CalculatorChallenge.Domain.Exceptions;

public sealed class TooManyNumbersException(int max) : Exception($"Maximum of {max} numbers is allowed.")
{
}
