using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using NoitaNET.API;
using NoitaNET.API.Lua;

namespace NoitaNET.Loader;

public static unsafe class Callbacks
{
    private static IReadOnlyList<Mod> Mods => ModLoadHandler.Mods;

    internal static LuaNative.lua_State* CurrentUnsafeLuaState;

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
            mod.EnableLuaState(CurrentUnsafeLuaState);
            mod.OnWorldPostUpdate();
            mod.DisableLuaState();
        }
    }

    [UnmanagedCallersOnly(CallConvs = new Type[] { typeof(CallConvCdecl) })]
    public static void OnModPreInit()
    {
        for (int i = 0; i < Mods.Count; i++)
        {
            Mod mod = Mods[i];
            mod.EnableLuaState(CurrentUnsafeLuaState);
            mod.OnModPreInit();
            mod.DisableLuaState();
        }
    }

    [UnmanagedCallersOnly(CallConvs = new Type[] { typeof(CallConvCdecl) })]
    public static void OnModInit()
    {
        for (int i = 0; i < Mods.Count; i++)
        {
            Mod mod = Mods[i];
            mod.EnableLuaState(CurrentUnsafeLuaState);
            mod.OnModInit();
            mod.DisableLuaState();
        }
    }

    [UnmanagedCallersOnly(CallConvs = new Type[] { typeof(CallConvCdecl) })]
    public static void OnModPostInit()
    {
        for (int i = 0; i < Mods.Count; i++)
        {
            Mod mod = Mods[i];
            mod.EnableLuaState(CurrentUnsafeLuaState);
            mod.OnModPostInit();
            mod.DisableLuaState();
        }
    }
}
