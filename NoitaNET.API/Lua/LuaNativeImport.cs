namespace NoitaNET.API.Lua;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
internal class LuaNativeImport : Attribute
{
    public LuaNativeImport()
    {

    }
}
