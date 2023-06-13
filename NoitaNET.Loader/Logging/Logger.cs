namespace NoitaNET.Loader.Logging;

public abstract class Logger
{
    public abstract bool IsEnabled(LogLevel logLevel);

    public abstract void Log(LogLevel logLevel, string message);
}