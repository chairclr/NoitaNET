namespace NoitaNET.API.Services;

public class PathService
{
    /// <summary>
    /// The directory that contains the NoitaNET.API.dll assembly
    /// </summary>
    public static readonly string WorkingDirectory;

    public static readonly string LogsDirectory;

    /// <summary>
    /// The Noita mods folds. What else did you expect?
    /// </summary>
    public static readonly string NoitaModsFolder;

    static PathService()
    {
        WorkingDirectory = Path.GetDirectoryName(typeof(Mod).Assembly.Location!)!;

        LogsDirectory = Path.Combine(WorkingDirectory, "Logs");

        NoitaModsFolder = Path.GetFullPath(Path.Combine(WorkingDirectory, "..", ".."));
    }
}
