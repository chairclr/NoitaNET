using System.Runtime.InteropServices;
using NoitaNET.API;
using NoitaNET.Loader.Services;

namespace NoitaNET.Loader;

public unsafe class EntryHandler
{
    public delegate void EntryDelegate(char** activeMods, int activeModsCount);

    /// <summary>
    /// Entry point function for the entire NoitaNET managed side
    /// </summary>
    public static void Entry(char** activeMods, int activeModsCount)
    {
        string[] managedActiveMods = new string[activeModsCount];

        // We can't directly pass a string[] from C++ to C#, so we must pass a char** and then retrieve the strings in C#
        for (int i = 0; i < managedActiveMods.Length; i++)
        {
            // TODO: Confirm that UTF8 is the right encoding when using "Multi-Byte Character Set"
            // Or switch to unicode lol
            managedActiveMods[i] = Marshal.PtrToStringUTF8((nint)activeMods[i])!;
        }

        List<ModDescription> mods = ModFinderService.FindMods(managedActiveMods);

        ModLoadHandler modLoadHandler = new ModLoadHandler(mods);

        modLoadHandler.LoadMods();
    }
}
