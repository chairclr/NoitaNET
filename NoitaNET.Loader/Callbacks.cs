using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using NoitaNET.API;

namespace NoitaNET.Loader;

internal static unsafe class Callbacks
{
    private static IReadOnlyList<Mod> Mods => ModLoadHandler.Mods;

    [UnmanagedCallersOnly(CallConvs = new Type[] { typeof(CallConvCdecl) })]
    public static void OnWorldPreUpdate()
    {
        for (int i = 0; i < Mods.Count; i++)
        {
            Mod mod = Mods[i];
            mod.OnWorldPreUpdate();
        }
    }

    [UnmanagedCallersOnly(CallConvs = new Type[] { typeof(CallConvCdecl) })]
    public static void OnWorldPostUpdate()
    {
        for (int i = 0; i < Mods.Count; i++)
        {
            Mod mod = Mods[i];
            mod.OnWorldPostUpdate();
        }
    }

    [UnmanagedCallersOnly(CallConvs = new Type[] { typeof(CallConvCdecl) })]
    public static void OnModPreInit()
    {
        for (int i = 0; i < Mods.Count; i++)
        {
            Mod mod = Mods[i];
            mod.OnModPreInit();
        }
    }

    [UnmanagedCallersOnly(CallConvs = new Type[] { typeof(CallConvCdecl) })]
    public static void OnModInit()
    {
        for (int i = 0; i < Mods.Count; i++)
        {
            Mod mod = Mods[i];
            mod.OnModInit();
        }
    }

    [UnmanagedCallersOnly(CallConvs = new Type[] { typeof(CallConvCdecl) })]
    public static void OnModPostInit()
    {
        for (int i = 0; i < Mods.Count; i++)
        {
            Mod mod = Mods[i];
            mod.OnModPostInit();
        }
    }
}
