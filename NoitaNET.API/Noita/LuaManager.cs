using NoitaNET.API.Lua;

namespace NoitaNET.API.Noita;

public unsafe class LuaManager
{
    internal readonly LuaNative.lua_State* L;

    public LuaManager()
    {
        L = LuaNative.luaL_newstate();
    }
}
