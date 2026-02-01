using CalculatorChallenge.Application.Interfaces;
using System.Reflection.PortableExecutable;
using System.Text.RegularExpressions;

namespace CalculatorChallenge.Application.Services
{
    public sealed class StringParserService : IParserService
    {
        public List<string> Parse(string? input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return ["0"];

            var (delimiters, body) = ExtractDelimiters(input);

            var parsedInput = SplitByDelimiters(input, delimiters);

            return parsedInput;
        }

        private static (List<string> delimiters, string body) ExtractDelimiters(string input)
        {
            var delimiters = new List<string> { ",", @"\n" };

            if (!input.StartsWith("//"))
                return (delimiters, input);

            var newlineIndex = input.IndexOf(@"\n");
            if (newlineIndex < 0)
                return (delimiters, input);

            var header = input.Substring(2, newlineIndex - 2);
            var body = input[(newlineIndex + 2)..];

            if (header.StartsWith("[") && header.EndsWith("]"))
            {
                var inner = header[1..^1];
                delimiters.Add(inner);
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
}