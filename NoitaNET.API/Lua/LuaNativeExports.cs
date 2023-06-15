using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace NoitaNET.API.Lua;

public partial class LuaNative
{
    private static nint LuaModule;

    static LuaNative()
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

    [DllImport("kernel32", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
    private static extern nint GetProcAddress(nint hModule, string procName);

    public static nint GetLuaExport(string name)
    {
        return GetProcAddress(LuaModule, name);
    }
}
