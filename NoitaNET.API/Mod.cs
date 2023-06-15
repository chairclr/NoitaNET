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

    public readonly Noita Noita;

    public Mod(string name, string description)
    {
        Name = name;

        Description = description;

        Noita = new Noita();
    }

    public virtual void OnWorldPreUpdate() { }

    public virtual void OnWorldPostUpdate() { }

    public virtual void OnModPreInit() { }

    public virtual void OnModInit() { }

    public virtual void OnModPostInit() { }
}
