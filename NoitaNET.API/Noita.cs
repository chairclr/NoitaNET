using NoitaNET.API.Lua;

namespace NoitaNET.API;

public unsafe class Noita
{
    private LuaNative.lua_State* LuaState;

    public Noita()
    {
        LuaState = LuaNative.luaL_newstate();
    }

    public List<int> EntityGetWithTag(string tag)
    {
        return new List<int>();
    }

    public void EntityKill(int id)
    {

    }
}
