using NoitaNET.API.Lua;

namespace NoitaNET.API.Noita;

/// <summary>
/// Represents an internal Lua state or environment
/// </summary>
/// <remarks>
/// Not thread safe
/// </remarks>
public unsafe class LuaManager
{
    internal readonly LuaNative.lua_State* L;

    public LuaManager()
    {
        L = LuaNative.luaL_newstate();
    }
}
