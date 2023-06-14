using NoitaNET.API.Logging;

namespace NoitaNET.Loader;

public static class Callbacks
{
    public static void OnWorldPreUpdate()
    {
        Logger.Instance.LogInformation("OnWorldPreUpdate()");
    }

    public static void OnWorldPostUpdate()
    {
        Logger.Instance.LogInformation("OnWorldPostUpdate()");
    }
}
