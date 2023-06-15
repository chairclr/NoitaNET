using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Security;

namespace NoitaNET.API.Lua;

public unsafe partial class LuaNative
{
    private static nint LuaModule;

    public static delegate* unmanaged[Cdecl, SuppressGCTransition]<lua_State*, int, double> Raw_lua_tonumber;
    public static delegate* unmanaged[Cdecl, SuppressGCTransition]<lua_State*, int, void> Raw_lua_settop;

    static LuaNative()
    {
        ProcessModuleCollection modules = Process.GetCurrentProcess().Modules;
        int count = modules.Count;
        for (int i = 0; i < count; i++)
        {
            if (modules[i].ModuleName == "lua51.dll")
            {
                LuaModule = modules[i].BaseAddress;
                break;
            }
        }

        Raw_lua_tonumber = (delegate* unmanaged[Cdecl, SuppressGCTransition]<lua_State*, int, double>)GetLuaExport("lua_tonumber");
        Raw_lua_settop = (delegate* unmanaged[Cdecl, SuppressGCTransition]<lua_State*, int, void>)GetLuaExport("lua_settop");
    }

    [DllImport("kernel32", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
    private static extern nint GetProcAddress(nint hModule, string procName);

    public static nint GetLuaExport(string name)
    {
        return GetProcAddress(LuaModule, name);
    }
}
