namespace NoitaNET.API.Logging;

public abstract class Logger
{
    public static Logger Instance { get; private set; } = new DefaultLogger();

    public abstract bool IsEnabled(LogLevel logLevel);

    public abstract void Log(LogLevel logLevel, string message);

    public void LogDebug(string message)
    {
        Log(LogLevel.Debug, message);
    }

    public void LogInformation(string message)
    {
        Log(LogLevel.Information, message);
    }

    public void LogWarning(string message)
    {
        Log(LogLevel.Warning, message);
    }

    public void LogError(string message)
    {
        Log(LogLevel.Error, message);
    }
}
