namespace NoitaNET.Loader.Services;

public class PathService
{
    public static readonly string WorkingDirectory = Path.GetDirectoryName(typeof(Loader).Assembly.Location);
}
