using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using NoitaNET.API;
using NoitaNET.API.Logging;
using NoitaNET.API.Lua;
using NoitaNET.API.Noita;
using static NoitaNET.API.Lua.LuaNative;

namespace NoitaNET.TestMod;

internal class Benchmarks
{
    public EngineAPI RawEngineAPI;

    public Benchmarks(EngineAPI engineAPI)
    {
        RawEngineAPI = engineAPI;
    }

    public void BenchmarkPureRandomCalls(long n)
    {
        Stopwatch sw = Stopwatch.StartNew();

        double result = 1;
        for (long i = 0; i < n; i++)
        {
            RawEngineAPI.Random(out result);
        }
        sw.Stop();

        Log(nameof(BenchmarkPureRandomCalls), n, sw);

        Consume(result);
    }

    public void BenchmarkGetPlayerByTag(long n)
    {
        Stopwatch sw = Stopwatch.StartNew();

        nint result = 1;
        for (long i = 0; i < n; i++)
        {
            RawEngineAPI.EntityGetWithTag("player_unit", out nint[] results);
            result = results[0];
        }
        sw.Stop();

        Log(nameof(BenchmarkGetPlayerByTag), n, sw);

        Consume(result);
    }

    [DllImport("lua51.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static unsafe extern void lua_pushstring(LuaNative.lua_State* L, string s);

    [DllImport("lua51.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static unsafe extern nint lua_tolstring(LuaNative.lua_State* L, int idx, ref nuint len);

    public unsafe void BenchmarkPushStringAndPopNew(long n)
    {
        Stopwatch sw = Stopwatch.StartNew();

        LuaNative.lua_State* L = LuaNative.luaL_newstate();
        string result = "";
        for (long i = 0; i < n; i++)
        {
            LuaNative.lua_pushstring(L, new string('x', 128));
            result = LuaNative.lua_tostring(L, -1)!;
            LuaNative.lua_settop(L, 0);
        }
        sw.Stop();

        Log(nameof(BenchmarkPushStringAndPopNew), n, sw);

        Consume(result);
    }

    public unsafe void BenchmarkPushStringAndPopOld(long n)
    {
        Stopwatch sw = Stopwatch.StartNew();

        LuaNative.lua_State* L = LuaNative.luaL_newstate();
        string result = "";
        for (long i = 0; i < n; i++)
        {
            lua_pushstring(L, new string('x', 128));
            nuint temp = 0;
            result = Marshal.PtrToStringUTF8(lua_tolstring(L, -1, ref temp))!;
            LuaNative.lua_settop(L, 0);
        }
        sw.Stop();

        LuaNative.lua_close(L);

        Log(nameof(BenchmarkPushStringAndPopOld), n, sw);

        Consume(result);
    }

    private void Log(string fn, long n, Stopwatch sw)
    {
        Logger.Instance.LogInformation($"[Benchmarks]: {fn}({n}) took {sw.Elapsed.TotalMilliseconds:F5}ms");
    }

    private object? __consumedObject;

    [MethodImpl(MethodImplOptions.NoInlining)]
    private void Consume(object? obj)
    {
        __consumedObject = obj;
    }
}
