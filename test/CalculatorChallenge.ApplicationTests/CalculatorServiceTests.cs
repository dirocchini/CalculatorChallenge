using CalculatorChallenge.Application.Interfaces;
using CalculatorChallenge.Application.Operations;
using CalculatorChallenge.Application.Options;
using CalculatorChallenge.Application.Services;
using CalculatorChallenge.Domain.Exceptions;
using Microsoft.Extensions.Options;

namespace CalculatorChallenge.ApplicationTests;

public sealed class CalculatorServiceTests
{
    private static ICalculatorService CreateSut(
        string operation = "add",
        string altDelimiter = @"\n",
        int maxValue = 1000,
        bool allowNegatives = false)
    {
        var options = Options.Create(new ParserOptions
        {
            Operation = operation,
            AlternateDelimiter = altDelimiter,
            MaxValue = maxValue,
            AllowNegatives = allowNegatives
        });

        IParserService parser = new StringParserService(options);

        var strategies = new IOperationStrategy[]
        {
            new AdditionOperation(),
            new SubtractionStrategy(),
            new MultiplicationOperation(),
            new DivisionOperation()
        };

        return new CalculatorService(parser, options, strategies);
    }

    [Theory]
    [InlineData(null, 0)]
    [InlineData("", 0)]
    [InlineData(" ", 0)]
    public void WhenEmptyValue_ShouldConvertToZero(string? input, int expected)
    {
        var sut = CreateSut();
        Assert.Equal(expected, sut.Calculate(input).Result);
        Assert.Equal("0", sut.Calculate(input).Formula);
    }

    [Theory]
    [InlineData("15", 15)]
    [InlineData("5,tytyt", 5)]
    [InlineData("7,", 7)]
    public void WhenMissingOrInvalidValues_ShouldConvertToZero(string input, int expected)
    {
        var sut = CreateSut();
        Assert.Equal(expected, sut.Calculate(input).Result);
    }

    [Theory]
    [InlineData("20", 20)]
    [InlineData("1,1000", 1001)]
    [InlineData("4,3", 7)]
    public void WhenNoErrors_ShouldCalculate(string input, int expected)
    {
        var sut = CreateSut();
        Assert.Equal(expected, sut.Calculate(input).Result);
    }

    [Fact]
    public void WhenManyNumbers_ShouldCalculate()
    {
        var sut = CreateSut();
        Assert.Equal(78, sut.Calculate("1,2,3,4,5,6,7,8,9,10,11,12").Result);
    }

    [Fact]
    public void WhenNewLineAsSeparator_ShouldCalculate()
    {
        var sut = CreateSut();
        Assert.Equal(6, sut.Calculate(@"1\n2,3").Result);
    }

    [Fact]
    public void WhenNegativesNumber_ShouldThrowExceptionWithNegatives()
    {
        var sut = CreateSut(allowNegatives: false);

        var ex = Assert.Throws<NegativeNumbersNotAllowedException>(() => sut.Calculate("1,-2,-3,4"));
        Assert.Contains("-2", ex.Message);
        Assert.Contains("-3", ex.Message);
    }

    [Fact]
    public void WhenNumberGreaterThanMaxValue_ShouldIgnoreGreaterValues()
    {
        var sut = CreateSut(maxValue: 1000);
        Assert.Equal(8, sut.Calculate("2,1001,6").Result);
        Assert.Equal("2+0+6", sut.Calculate("2,1001,6").Formula);
    }

    [Theory]
    [InlineData(@"//#\n2#5", 7)]
    [InlineData(@"//,\n2,ff,100", 102)]
    public void WhenCustomDelimiter_ShouldCalculate(string input, int expected)
    {
        var sut = CreateSut();
        Assert.Equal(expected, sut.Calculate(input).Result);
    }

    [Fact]
    public void WhenOneCustomHeader_ShouldCalculate()
    {
        var sut = CreateSut();
        Assert.Equal(66, sut.Calculate(@"//[***]\n11***22***33").Result);
    }

    [Fact]
    public void WhenMultipleCustomHeader_ShouldCalculate()
    {
        var sut = CreateSut();
        Assert.Equal(110, sut.Calculate(@"//[*][!!][r9r]\n11r9r22*hh*33!!44").Result);
    }

    [Theory]
    [InlineData("20", "20")]
    [InlineData("1,1000", "1+1000")]
    [InlineData("4,3,1001,2", "4+3+0+2")]
    public void WhenCalculate_ShouldDisplayCorrectFormula(string input, string expected)
    {
        var sut = CreateSut("add");
        Assert.Equal(expected, sut.Calculate(input).Formula);
    }

    [Fact]
    public void WhenCustomAlternateDelimiter_ShouldBeRespected()
    {
        var sut = CreateSut(operation: "add", altDelimiter: ";", maxValue: 1000, allowNegatives: false);

        var result = sut.Calculate("1;2,3");

        Assert.Equal("1+2+3", result.Formula);
        Assert.Equal(6, result.Result);
    }

    [Fact]
    public void WhenCustomMaxValue_ShouldBeRespected()
    {
        var sut = CreateSut(operation: "add", altDelimiter: ";", maxValue: 500, allowNegatives: false);

        var result = sut.Calculate("1;501,3");

        Assert.Equal("1+0+3", result.Formula);
        Assert.Equal(4, result.Result);
    }

    [Fact]
    public void WhenCustomNegativeNumbersFlagValue_ShouldBeRespected()
    {
        var sut = CreateSut(operation: "add", altDelimiter: ";", maxValue: 500, allowNegatives: true);

        var result = sut.Calculate("2;-1,3");

        Assert.Equal("2+-1+3", result.Formula);
        Assert.Equal(4, result.Result);
    }

    [Fact]
    public void WhenAddOperation_ShouldUsePlusInFormula()
    {
        var sut = CreateSut("add");
        var result = sut.Calculate("10,3,2");

        Assert.Equal("10+3+2", result.Formula);
        Assert.Equal(15, result.Result);
    }

    [Fact]
    public void WhenSubOperation_ShouldUseMinusInFormula()
    {
        var sut = CreateSut("sub");
        var result = sut.Calculate("10,3,2");

        Assert.Equal("10-3-2", result.Formula);
        Assert.Equal(5, result.Result);
    }

    [Fact]
    public void WhenMulOperation_ShouldUseAsteriskInFormula()
    {
        var sut = CreateSut("mul");
        var result = sut.Calculate("2,3,4");

        Assert.Equal("2*3*4", result.Formula);
        Assert.Equal(24, result.Result);
    }

    [Fact]
    public void WhenDivOperation_ShouldUseSlashInFormula()
    {
        var sut = CreateSut("div");
        var result = sut.Calculate("20,2,2");

        Assert.Equal("20/2/2", result.Formula);
        Assert.Equal(5, result.Result);
    }

    [Fact]
    public void WhenDivOperationWithDecimalResult_ShouldUseSlashInFormula()
    {
        var sut = CreateSut("div");
        var result = sut.Calculate("5,4,2");

        Assert.Equal("5/4/2", result.Formula);
        Assert.Equal(0.625m, result.Result);
    }

    [Fact]
    public void WhenDivOperationWithNegativeDecimalResult_ShouldUseSlashInFormula()
    {
        var sut = CreateSut(operation: "div", allowNegatives: true);
        var result = sut.Calculate("5,4,-2");

        Assert.Equal("5/4/-2", result.Formula);
        Assert.Equal(-0.625m, result.Result);
    }

    [Fact]
    public void WhenDivOperationByZero_ShouldThrowException()
    {
        var sut = CreateSut("div");
        Assert.Throws<DivideByZeroException>(() => sut.Calculate("10,0,2"));
    }
}
