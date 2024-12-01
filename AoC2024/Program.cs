using Common.AoC;
using Spectre.Console;
using Spectre.Console.Cli;

try
{
    var app = new CommandApp<AoCCommand>();
    app.Configure(x => x.PropagateExceptions());
    await app.RunAsync(args);
}
catch (Exception ex)
{
    AnsiConsole.WriteException(ex, ExceptionFormats.ShortenEverything);
}
