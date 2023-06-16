using unsafe APIFunction = delegate* unmanaged[Cdecl, SuppressGCTransition]<NoitaNET.API.Lua.LuaNative.lua_State*, void>;

namespace NoitaNET.API;

internal unsafe class EngineAPIFunctionTable
{
    // All engine api functions are set via reflection by the loader

    //public static APIFunction EntityLoad = default;

    //public static APIFunction EntityKill = default;

    //public static APIFunction EntityGetWithTag = default;

    //public static APIFunction Random = default;
}
