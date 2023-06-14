using NoitaNET.API;
using NoitaNET.API.Logging;

namespace NoitaNET.Loader;

public static class Callbacks
{
    private static IReadOnlyList<Mod> Mods => ModLoadHandler.Mods;

    public static void OnWorldPreUpdate()
    {
        for (int i = 0; i < Mods.Count; i++)
        {
            Mods[i].OnWorldPreUpdate();
        }
    }

    public static void OnWorldPostUpdate()
    {
        for (int i = 0; i < Mods.Count; i++)
        {
            Mods[i].OnWorldPostUpdate();
        }
    }
}
