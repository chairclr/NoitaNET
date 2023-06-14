namespace NoitaNET.Loader.Services;

public class PathService
{
    public static readonly string WorkingDirectory = Path.GetDirectoryName(typeof(EntryHandler).Assembly.Location!)!;

    public static string LogsDirectory => Path.Combine(WorkingDirectory, "Logs");

    public static string NoitaModsFolder => Path.GetFullPath(Path.Combine(WorkingDirectory, "..", ".."));
}
