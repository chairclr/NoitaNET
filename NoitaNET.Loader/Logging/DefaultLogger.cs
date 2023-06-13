using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using NoitaNET.Loader.Services;

namespace NoitaNET.Loader.Logging;

internal class DefaultLogger : Logger
{
    private readonly FileStream? FileStream;

    public DefaultLogger()
    {
        if (!Directory.Exists(PathService.LogsDirectory))
        {
            Directory.CreateDirectory(PathService.LogsDirectory);

            LogInformation($"Created Logs directory {PathService.LogsDirectory}");
        }

        string logFilePath = Path.Combine(PathService.LogsDirectory, $"NoitaNET.Loader-{DateTime.UtcNow:yyyy-MM-dd_HH-mm-ss}.txt");

        if (File.Exists(logFilePath))
        {
            return;
        }

        try
        {
            FileStream = new FileStream(logFilePath, FileMode.OpenOrCreate);
        }
        catch (UnauthorizedAccessException uae)
        {
            Log(LogLevel.Error, $"Could not create {logFilePath}: {uae.Message}");
        }
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
        if (!IsEnabled(logLevel))
        {
            return;
        }

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

        string formatted = $"[{logLevel}] {new string(' ', 11 - logLevel.ToString().Length)}{message}";

        Console.WriteLine(formatted);

        if (FileStream is not null)
        {
            FileStream.Write(MemoryMarshal.AsBytes(formatted.AsSpan()));
            FileStream.Flush();
        }
    }
}
