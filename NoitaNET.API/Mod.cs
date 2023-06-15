using NoitaNET.API.Lua;

namespace NoitaNET.API;

/// <summary>
/// Represents a NoitaNET mod. Inherit from this class and annotate your class with the <see cref="ModEntryAttribute"/> attribute.
/// </summary>
/// <remarks>
/// Your mod is constructed when it is loaded.
/// 
/// Your mod class must be public.
/// </remarks>
public abstract unsafe class Mod
{
    public readonly string Name;

    public readonly string Description;

    private LuaNative.lua_State* UnsafeState;

    public bool LuaStateAvailable { get; internal set; }

    public LuaNative.lua_State* LuaState => LuaStateAvailable ? UnsafeState : null;

    public Mod(string name, string description)
    {
        Name = name;

        Description = description;
    }

    public virtual void OnWorldPreUpdate() { }

    public virtual void OnWorldPostUpdate() { }

    public virtual void OnModPreInit() { }

    public virtual void OnModInit() { }

    public virtual void OnModPostInit() { }

    public void EnableLuaState(LuaNative.lua_State* luaState)
    {
        if (luaState != null)
        {
            LuaStateAvailable = true;
            UnsafeState = luaState;
        }
    }

    public void DisableLuaState()
    {
        LuaStateAvailable = false;
    }
}
