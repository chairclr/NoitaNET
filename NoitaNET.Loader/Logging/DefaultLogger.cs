using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NoitaNET.Loader.Services;

namespace NoitaNET.Loader.Logging;

internal class DefaultLogger : Logger
{
    private readonly FileStream FileStream;

    public DefaultLogger()
    {

        //FileStream = new FileStream(Path.Combine(PathService.WorkingDirectory, $"Coolua"));
    }

    public override bool IsEnabled(LogLevel logLevel)
    {
#if !DEBUG
        if (logLevel > LogLevel.Debug)
        {
            return true;
        }

        return false;
#else
        return true;
#endif
    }

    public override void Log(LogLevel logLevel, string message)
    {
        switch (logLevel)
        {
            case LogLevel.Debug:
            case LogLevel.Information:
                Console.ForegroundColor = ConsoleColor.DarkGray;
                break;
            case LogLevel.Warning:
                Console.ForegroundColor = ConsoleColor.Yellow;
                break;
            case LogLevel.Error:
                Console.ForegroundColor = ConsoleColor.Red;
                break;
        }

        Console.WriteLine($"[{logLevel}] {new string(' ', 11 - logLevel.ToString().Length)}{message}");
    }
}
