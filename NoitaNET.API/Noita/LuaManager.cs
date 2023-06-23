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
    internal readonly ThreadLocal<LuaStateContainer> ThreadLocalLuaState;

    internal readonly ThreadLocal<EngineAPI> ThreadLocalEngineAPI;

    public LuaManager()
    {
        ThreadLocalLuaState = new ThreadLocal<LuaStateContainer>(() => new LuaStateContainer());

        ThreadLocalEngineAPI = new ThreadLocal<EngineAPI>(() => new EngineAPI(ThreadLocalLuaState.Value!));
    }

    internal class LuaStateContainer
    {
        internal readonly LuaNative.lua_State* L;

        public LuaStateContainer()
        {
            L = LuaNative.luaL_newstate();
        }
    }
}
