using System.Numerics;
using System.Runtime.InteropServices;
using System.Security;
using NoitaNET.API.Lua;

namespace NoitaNET.API;

public unsafe partial class Noita
{
    private readonly LuaNative.lua_State* L;

    public readonly EngineAPI RawAPI;

    public Noita()
    {
        L = LuaNative.luaL_newstate();

        RawAPI = new EngineAPI(this);
    }
}