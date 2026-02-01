using CalculatorChallenge.Application.Interfaces;
using CalculatorChallenge.Domain;
using System.Text.RegularExpressions;

namespace CalculatorChallenge.Application.Services
{
    public sealed class StringParserService : IParserService
    {
        public List<string> Parse(string? input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return ["0"];

            var allDelimiters = new List<string> { ",", @"\n" };

            var parsedInput = SplitByDelimiters(input, allDelimiters);

            return parsedInput;
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
}