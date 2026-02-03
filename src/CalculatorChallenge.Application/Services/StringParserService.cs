using CalculatorChallenge.Application.Interfaces;
using CalculatorChallenge.Application.Options;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Options;

namespace CalculatorChallenge.Application.Services;

public sealed class StringParserService : IParserService
{
    private readonly ParserOptions _options;

    public StringParserService(IOptions<ParserOptions> options)
    {
        _options = options.Value;
    }

    public List<string> Parse(string? input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return ["0"];

        var (delimiters, body) = ExtractDelimiters(input);

        var parsedInput = SplitByDelimiters(body, delimiters);

        return parsedInput;
    }

    private (List<string> delimiters, string body) ExtractDelimiters(string input)
    {
        var delimiters = new List<string> { ",", _options.AlternateDelimiter };

        if (!input.StartsWith("//"))
            return (delimiters, input);

        var newlineIndex = input.IndexOf(@"\n");
        if (newlineIndex < 0)
            return (delimiters, input);

        var header = input.Substring(2, newlineIndex - 2);
        var body = input[(newlineIndex + 2)..];

        if (header.StartsWith("["))
        {
            var matches = Regex.Matches(header, @"\[(.*?)\]");
            foreach (Match m in matches)
            {
                var d = m.Groups[1].Value;
                if (!string.IsNullOrEmpty(d))
                    delimiters.Add(d);
            }
            return (delimiters, body);
        }

        if (header.Length == 1)
        {
            delimiters.Add(header);
            return (delimiters, body);
        }

        return (delimiters, input);
    }

    private static List<string> SplitByDelimiters(string input, List<string> delimiters)
    {
        var pattern = string.Join("|", delimiters
            .Where(d => !string.IsNullOrEmpty(d))
            .OrderByDescending(d => d.Length)
            .Select(Regex.Escape));

        return [.. Regex.Split(input, pattern)];
    }
}