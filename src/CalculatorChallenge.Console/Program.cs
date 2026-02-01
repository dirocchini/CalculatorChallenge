using CalculatorChallenge.Application.Interfaces;
using CalculatorChallenge.Application.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;


using var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddSingleton<ICalculatorService, CalculatorService>();
    })
    .Build();

var calc = host.Services.GetRequiredService<ICalculatorService>();

Console.WriteLine("String Calculator (Ctrl+C para sair).");
while (true)
{
    Console.Write("> ");
    var line = Console.ReadLine();
    Console.WriteLine(calc.Calculate(line));
}