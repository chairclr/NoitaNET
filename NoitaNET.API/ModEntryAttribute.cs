namespace NoitaNET.API;

/// <summary>
/// An attribute to annotate the class to instaniate when your mod is loaded
/// </summary>
[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
public class ModEntryAttribute : Attribute
{
    public ModEntryAttribute()
    {

    }
}
