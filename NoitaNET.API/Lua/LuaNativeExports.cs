using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Security;
using NoitaNET.API.Logging;

namespace NoitaNET.API.Lua;

public unsafe partial class LuaNative
{
    private static nint LuaModule;

    [DllImport("kernel32", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
    private static extern nint GetProcAddress(nint hModule, string procName);

    public static nint GetLuaExport(string name)
    {
        if (LuaModule == 0)
        {
            ProcessModuleCollection modules = Process.GetCurrentProcess().Modules;
            int count = modules.Count;
            for (int i = 0; i < count; i++)
            {
                if (modules[i].ModuleName == "lua51.dll")
                {
                    LuaModule = modules[i].BaseAddress;
                    break;
                }
            }
        }

        if (LuaModule == 0)
        {
            Exception e = new Exception();
            Logger.Instance.LogError($"Failed to get lua51.dll base address; {e}");
            throw e;
        }

        return GetProcAddress(LuaModule, name);
    }
}
