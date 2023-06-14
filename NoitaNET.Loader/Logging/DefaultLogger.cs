using System.Runtime.InteropServices;
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

        // Just give up at this point
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

        // We want all of the log messages to be aligned
        // Longest length is "Information" at 11 characters long
        string formatted = $"[{logLevel}] {new string(' ', 11 - logLevel.ToString().Length)}{message}";

        Console.WriteLine(formatted);

        if (FileStream is not null)
        {
            // It works lol
            FileStream.Write(MemoryMarshal.AsBytes((formatted + "\n").AsSpan()));
            FileStream.Flush();
        }
    }
}
