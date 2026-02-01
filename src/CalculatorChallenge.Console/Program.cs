using CalculatorChallenge.Application.Interfaces;
using CalculatorChallenge.Application.Options;
using CalculatorChallenge.Application.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;


static string? GetArg(string[] args, string key)
{
    var arg = args.FirstOrDefault(a => a.StartsWith(key + "="));
    return arg?.Split("=", 2)[1];
}

using var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.Configure<ParserOptions>(options =>
        {
            options.AlternateDelimiter = @"\n";

            var alt = GetArg(args, "--alt-delimiter");
            if (!string.IsNullOrEmpty(alt))
                options.AlternateDelimiter = alt;

            var maxValue = GetArg(args, "--max-value");
            if (int.TryParse(maxValue, out var parsedMax))
                options.MaxValue = parsedMax;

            var allowNegatives = GetArg(args, "--allow-negatives");
            if (bool.TryParse(allowNegatives, out var parsedAllow))
                options.AllowNegatives = parsedAllow;
        });

        services.AddSingleton<ICalculatorService, CalculatorService>();
        services.AddSingleton<IParserService, StringParserService>();
    })
    .Build();

var calc = host.Services.GetRequiredService<ICalculatorService>();

Console.WriteLine("String Calculator (Ctrl+C para sair).");
while (true)
{
    Console.Write("> ");
    var line = Console.ReadLine();

    try
    {
        var result = calc.Calculate(line);
        Console.WriteLine("Result: " + result.Result);
        Console.WriteLine("Formula: " + result.Formula);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"ERROR: {ex.Message}");
    }
}