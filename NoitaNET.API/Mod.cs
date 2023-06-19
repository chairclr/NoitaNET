using NoitaNET.API.Noita;

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

    // We use ThreadLocal here because the user might try to access the RawEngineAPI from another thread
    // Lua states are NOT thread safe
    private readonly ThreadLocal<LuaManager> ThreadLocalLuaManager;

    private readonly ThreadLocal<EngineAPI> ThreadLocalEngineAPI;

    public LuaManager LuaManager => ThreadLocalLuaManager.Value!;

    public EngineAPI RawEngineAPI => ThreadLocalEngineAPI.Value!;

    public Mod(string name, string description)
    {
        Name = name;

        Description = description;

        ThreadLocalLuaManager = new ThreadLocal<LuaManager>(() => new LuaManager());

        ThreadLocalEngineAPI = new ThreadLocal<EngineAPI>(() => new EngineAPI(LuaManager));
    }

    public virtual void OnWorldPreUpdate() { }

    public virtual void OnWorldPostUpdate() { }

    public virtual void OnModPreInit() { }

    public virtual void OnModInit() { }

    public virtual void OnModPostInit() { }
}
