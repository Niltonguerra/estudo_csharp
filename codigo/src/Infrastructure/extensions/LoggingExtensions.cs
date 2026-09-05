using Serilog;

namespace ProductsApi.Infrastructure.Extensions;

public static class LoggingExtensions
{
    //.WriteTo.File("logs/app.log", rollingInterval: RollingInterval.Day) — 
    // adiciona um arquivo como destino. O rollingInterval: RollingInterval.Day 
    // significa que a cada dia cria um novo arquivo automaticamente:
    public static void AddLogging()
    {
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .WriteTo.File("logs/app.log", rollingInterval: RollingInterval.Day)
            .CreateLogger();
    }
}

