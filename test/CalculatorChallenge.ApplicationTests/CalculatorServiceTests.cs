using CalculatorChallenge.Application.Interfaces;
using CalculatorChallenge.Application.Services;
using CalculatorChallenge.Domain.Exceptions;

namespace CalculatorChallenge.ApplicationTests;

public class CalculatorServiceTests
{
    private readonly ICalculatorService _calculatorService = new CalculatorService(new StringParserService());

    [Theory]
    [InlineData(null, 0)]
    [InlineData("", 0)]
    [InlineData(" ", 0)]
    public void WhenEmptyValue_ShouldConvertToZero(string? input, int expected) => Assert.Equal(expected, _calculatorService.Calculate(input).Result);

    [Theory]
    [InlineData("15", 15)]
    [InlineData("5,tytyt", 5)]
    [InlineData("7,", 7)]
    public void WhenMissingOrInvalidValues_ShouldConvertToZero(string? input, int expected) => Assert.Equal(expected, _calculatorService.Calculate(input).Result);

    [Theory]
    [InlineData("20", 20)]
    [InlineData("1,1000", 1001)]
    [InlineData("4,3", 7)]
    public void WhenNoErrors_ShouldCalculate(string? input, int expected) => Assert.Equal(expected, _calculatorService.Calculate(input).Result);

    [Fact]
    public void WhenManyNumbers_ShouldCalculate()
       => Assert.Equal(78, _calculatorService.Calculate("1,2,3,4,5,6,7,8,9,10,11,12").Result);

    [Fact]
    public void WhenNewLineAsSeparator_ShouldCalculate()
       => Assert.Equal(6, _calculatorService.Calculate(@"1\n2,3").Result);

    [Fact]
    public void WhenNegativesNumber_ShouldThrowExceptionWithNegatives()
    {
        var ex = Assert.Throws<NegativeNumbersNotAllowedException>(() => _calculatorService.Calculate("1,-2,-3,4"));
        Assert.Contains("-2", ex.Message);
        Assert.Contains("-3", ex.Message);
    }

    [Fact]
    public void WhenNumberGreaterThan1000L_ShouldIgnoreGreaterValuesThan1000()
        => Assert.Equal(8, _calculatorService.Calculate(@"2,1001,6").Result);

    [Theory]
    [InlineData(@"//#\n2#5", 7)]
    [InlineData(@"//,\n2,ff,100", 102)]
    public void WhenCustomDelimiter_ShouldCalculate(string? input, int expected) => Assert.Equal(expected, _calculatorService.Calculate(input).Result);

    [Fact]
    public void WhenOneCustomHeader_ShouldCalculate()
      => Assert.Equal(66, _calculatorService.Calculate(@"//[***]\n11***22***33").Result);

    [Fact]
    public void WhenMultipleCustomHeader_ShouldCalculate()
        => Assert.Equal(110, _calculatorService.Calculate(@"//[*][!!][r9r]\n11r9r22*hh*33!!44").Result);

    [Theory]
    [InlineData("20", "20")]
    [InlineData("1,1000", "1+1000")]
    [InlineData("4,3,1001,2", "4+3+0+2")]
    public void WhenCalulate_ShouldDisplayCorrectFormula(string? input, string expected) => Assert.Equal(expected, _calculatorService.Calculate(input).Formula);
}
