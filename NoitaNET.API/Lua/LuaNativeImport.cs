namespace NoitaNET.API.Lua;

/// <summary>
/// Attribute use to identify which functions from LuaNative should be imported as raw function pointers and do not suppress GC transition
/// </summary>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
internal class LuaNativeImport : Attribute
{
    public LuaNativeImport()
    {

    }
}
