namespace NoitaNET.Loader.Services;

public class PathService
{
    public static readonly string WorkingDirectory = Path.GetDirectoryName(typeof(Loader).Assembly.Location!)!;

    public static string LogsDirectory => Path.Combine(PathService.WorkingDirectory, "Logs");
}
