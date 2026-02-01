using CalculatorChallenge.Application.Interfaces;
using CalculatorChallenge.Application.Services;
using CalculatorChallenge.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace CalculatorChallenge.ApplicationTests;

public class CalculatorServiceTests
{
    private readonly ICalculatorService _calculatorService = new CalculatorService();

    [Theory]
    [InlineData(null, 0)]
    [InlineData("", 0)]
    [InlineData(" ", 0)]
    public void WhenEmptyValue_ShouldConvertToZero(string? input, int expected) => Assert.Equal(expected, _calculatorService.Calculate(input));

    [Theory]
    [InlineData("15", 15)]
    [InlineData("5,tytyt", 5)]
    [InlineData("7,", 7)]
    public void WhenMissingOrInvalidValues_ShouldConvertToZero(string? input, int expected) => Assert.Equal(expected, _calculatorService.Calculate(input));

    [Fact]
    public void WhenMoreThanTwoNumbersProvided_ShouldThrowException()
       => Assert.Throws<TooManyNumbersException>(() => _calculatorService.Calculate("1,2,3"));

    [Theory]
    [InlineData("20", 20)]
    [InlineData("1,5000", 5001)]
    [InlineData("4,-3", 1)]
    public void WhenNoErrors_ShouldCalculate(string? input, int expected) => Assert.Equal(expected, _calculatorService.Calculate(input));
}
