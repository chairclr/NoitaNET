using NoitaNET.Loader.Logging;

namespace NoitaNET.Loader;

public class Loader
{
    public delegate void EntryDelegate();

    public static void Entry()
    {
        Logger.Instance.LogInformation("Successfully loaded NoitaNET.Loader");
    }
}
