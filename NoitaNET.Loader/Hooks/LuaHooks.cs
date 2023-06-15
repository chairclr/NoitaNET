using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using NoitaNET.API.Hooks;
using NoitaNET.API.Logging;
using NoitaNET.API.Lua;

namespace NoitaNET.Loader.Hooks;

internal unsafe class LuaHooks
{
    public static void Register()
    {
        delegate* unmanaged[Cdecl]<LuaNative.lua_State*, int, int> lua_type_Detour = &Hook_lua_type;
        NativeHook NativeHook_lua_type = new NativeHook(LuaNative.GetLuaExport("lua_type"), (nint)lua_type_Detour);
        Original_lua_type = (delegate* unmanaged[Cdecl]<LuaNative.lua_State*, int, int>)NativeHook_lua_type.Original;
    }

    private static delegate* unmanaged[Cdecl]<LuaNative.lua_State*, int, int> Original_lua_type;

    [UnmanagedCallersOnly(CallConvs = new Type[] { typeof(CallConvCdecl) })]
    private static int Hook_lua_type(LuaNative.lua_State* L, int idx)
    {
        Callbacks.CurrentUnsafeLuaState = L;

        //Logger.Instance.LogDebug($"Got Lua State: {(nint)L:X}");

        return Original_lua_type(L, idx);
    }
}
