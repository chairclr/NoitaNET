using System.Numerics;
using System.Runtime.InteropServices;
using System.Security;
using NoitaNET.API.Lua;

namespace NoitaNET.API;

public unsafe partial class Noita
{
    private LuaNative.lua_State* L;

    public Noita()
    {
        L = LuaNative.luaL_newstate();
    }

    public void EntityKill(int id)
    {
        LuaNative.lua_pushnumber(L, id);
        EngineAPIFunctionTable.EntityGetWithTag(L);
        LuaNative.lua_settop(L, 0);
    }

    public long[] EntityGetWithTag(string tag)
    {
        LuaNative.lua_pushstring(L, tag);
        EngineAPIFunctionTable.EntityGetWithTag(L);

        ulong length = LuaNative.lua_objlen(L, 1);

        long[] result = new long[length]; 

        for (int i = 0; (ulong)i < length; i++)
        {
            LuaNative.lua_rawgeti(L, -1, i);
            result[i] = LuaNative.lua_tointeger(L, -1);
        }

        LuaNative.lua_settop(L, 0);

        return result;
    }

    public double Random()
    {
        EngineAPIFunctionTable.Random(L);
        double n = LuaNative.lua_tonumber(L, -1);
        LuaNative.lua_settop(L, 0);
        return n;
    }
}
