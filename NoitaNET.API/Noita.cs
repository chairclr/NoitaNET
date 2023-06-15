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
        LuaNative.lua_pushcclosure(L, EngineAPIFunctionTable.EntityKill, 0);
        LuaNative.lua_pushnumber(L, id);
        LuaNative.lua_call(L, 1, 0);
        LuaNative.lua_settop(L, 0);
    }

    public int[] EntityGetWithTag(string tag)
    {
        LuaNative.lua_pushcclosure(L, EngineAPIFunctionTable.EntityGetWithTag, 0);
        LuaNative.lua_pushstring(L, tag);
        LuaNative.lua_call(L, 1, 1);

        int length = (int)LuaNative.lua_objlen(L, 1);

        int[] result = new int[length]; 

        for (int i = 0; i < length; i++)
        {
            // + 1 because lua tables are base 1 indexed (wtf?)
            LuaNative.lua_pushinteger(L, i + 1);
            LuaNative.lua_gettable(L, -2);
            int value = (int)LuaNative.lua_tointeger(L, -1);

            result[i] = value;
        }

        LuaNative.lua_pop(L, 1);

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
