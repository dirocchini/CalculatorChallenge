using CalculatorChallenge.Application.Interfaces;
using CalculatorChallenge.Application.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;


using var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
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
        Console.WriteLine(result);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"ERROR: {ex.Message}");
    }
}