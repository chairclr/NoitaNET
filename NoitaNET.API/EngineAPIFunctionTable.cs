using NoitaNET.API.Lua;

namespace NoitaNET.API;

internal unsafe class EngineAPIFunctionTable
{
    // All engine api functions are set via reflection by the loader

    public static nint EntityKill = 0;

    public static nint EntityGetWithTag = 0;

    public static delegate* unmanaged[Cdecl, SuppressGCTransition]<LuaNative.lua_State*, void> Random = default;
}
