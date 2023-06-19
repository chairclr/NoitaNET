using System.Runtime.InteropServices;

namespace NoitaNET.API.Lua;

#pragma warning disable SYSLIB1054
#pragma warning disable IDE1006
#pragma warning disable CA1401

/// <summary>
/// Defines luajit api functions and primitives
/// </summary>
public unsafe partial class LuaNative
{
    private const string DllName = "lua51.dll";
    private const CallingConvention Convention = CallingConvention.Cdecl;

    public struct lua_State
    {

    }

    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
    public struct lua_Debug
    {
        public int _event;
        public string name;
        public string namewhat;
        public string what;
        public string source;
        public int currentline;
        public int nups;
        public int linedefined;
        public int lastlinedefined;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = LUA_IDSIZE)]
        public sbyte[] short_src;
        public int i_ci;
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
    public struct luaL_Reg
    {
        public string name;
        public lua_CFunction func;
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
    public struct luaL_Buffer
    {
        public char* p;
        public int lvl;
        public lua_State* L;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = LUAL_BUFFERSIZE)]
        public sbyte[] buffer;
    };

    public delegate int lua_CFunction(lua_State* L);
    public delegate char* lua_Reader(lua_State* L, nint ud, ref nint sz);
    public delegate int lua_Writer(lua_State* L, nint p, nint sz, nint ud);
    public delegate nint lua_Alloc(nint ud, nint ptr, nint osize, nint nsize);
    public delegate void lua_Hook(lua_State* L, lua_Debug ar);

    public const int LUAJIT_VERSION_NUM = 20100;
    public const int LUAJIT_MODE_MASK = 0x00FF;

    public const int LUAJIT_MODE_ENGINE = 0;
    public const int LUAJIT_MODE_DEBUG = 1;
    public const int LUAJIT_MODE_FUNC = 2;
    public const int LUAJIT_MODE_ALLFUNC = 3;
    public const int LUAJIT_MODE_ALLSUBFUNC = 4;
    public const int LUAJIT_MODE_TRACE = 5;
    public const int LUAJIT_MODE_WRAPCFUNC = 0x10;
    public const int LUAJIT_MODE_MODE_MAX = 0x11;

    public const int LUAJIT_MODE_OFF = 0x0000;
    public const int LUAJIT_MODE_ON = 0x0100;
    public const int LUAJIT_MODE_FLUSH = 0x0200;

    public delegate void luaJIT_profile_callback(nint data, lua_State* L, int samples, int vmstate);

    [DllImport(DllName, CallingConvention = Convention)]
    public static extern void luaJIT_profile_start(lua_State* L, string mode, luaJIT_profile_callback cb, nint data);

    [LuaNativeImport]
    public static partial void luaJIT_profile_stop(lua_State* L);

    [DllImport(DllName, CallingConvention = Convention)]
    public static extern nint _luaJIT_profile_dumpstack(lua_State* L, string fmt, int depth, ref nint len);

    public static string? luaJIT_profile_dumpstack(lua_State* L, string fmt, int depth, ref nint len)
    {
        return Marshal.PtrToStringAnsi(_luaJIT_profile_dumpstack(L, fmt, depth, ref len));
    }

    [LuaNativeImport]
    public static partial void luaJIT_version_2_1_0_beta3();

    public const string LUA_LDIR = "!\\lua\\";
    public const string LUA_CDIR = "!\\";

    public const string LUA_PATH_DEFAULT = ".\\?.lua;" + LUA_LDIR + "?.lua;" + LUA_LDIR + "?\\init.lua;";
    public const string LUA_CPATH_DEFAULT = ".\\?.dll;" + LUA_CDIR + "?.dll;" + LUA_CDIR + "loadall.dll";

    public const string LUA_PATH = "LUA_PATH";
    public const string LUA_CPATH = "LUA_CPATH";
    public const string LUA_INIT = "LUA_INIT";

    public const string LUA_DIRSEP = "\\";
    public const string LUA_PATHSEP = ";";
    public const string LUA_PATH_MARK = "?";
    public const string LUA_EXECDIR = "!";
    public const string LUA_IGMARK = "-";
    public const string LUA_PATH_CONFIG = LUA_DIRSEP + "\n" + LUA_PATHSEP + "\n" + LUA_PATH_MARK + "\n" + LUA_EXECDIR + "\n" + LUA_IGMARK + "\n";

    public static string LUA_QL(string x)
    {
        return "'" + x + "'";
    }

    public const string LUA_QS = "'%s'";

    public const int LUAI_MAXSTACK = 65500;
    public const int LUAI_MAXCSTACK = 8000;
    public const int LUAI_GCPAUSE = 200;
    public const int LUAI_GCMUL = 200;
    public const int LUA_MAXCAPTURES = 32;

    public const int LUA_IDSIZE = 60;

    public const int LUAL_BUFFERSIZE = 512 > 16384 ? 8182 : 512;

    public const string LUA_VERSION = "Lua 5.1";
    public const string LUA_RELEASE = "Lua 5.1.4";
    public const int LUA_VERSION_NUM = 501;
    public const string LUA_COPYRIGHT = "Copyright (C) 1994-2008 Lua.org, PUC-Rio";
    public const string LUA_AUTHORS = "R. Ierusalimschy, L. H. de Figueiredo, W. Celes";

    public const string LUA_SIGNATURE = "\x1bLua";

    public const int LUA_MULTRET = -1;

    public const int LUA_REGISTRYINDEX = (-10000);
    public const int LUA_ENVIRONINDEX = (-10001);
    public const int LUA_GLOBALSINDEX = (-10002);

    public static int lua_upvalueindex(int i)
    {
        return LUA_GLOBALSINDEX - i;
    }

    public const int LUA_OK = 0;
    public const int LUA_YIELD = 1;
    public const int LUA_ERRRUN = 2;
    public const int LUA_ERRSYNTAX = 3;
    public const int LUA_ERRMEM = 4;
    public const int LUA_ERRERR = 5;

    public const int LUA_TNONE = -1;
    public const int LUA_TNIL = 0;
    public const int LUA_TBOOLEAN = 1;
    public const int LUA_TLIGHTUSERDATA = 2;
    public const int LUA_TNUMBER = 3;
    public const int LUA_TSTRING = 4;
    public const int LUA_TTABLE = 5;
    public const int LUA_TFUNCTION = 6;
    public const int LUA_TUSERDATA = 7;
    public const int LUA_TTHREAD = 8;

    public const int LUA_MINSTACK = 20;

    [LuaNativeImport]
    public static partial lua_State* lua_newstate(lua_Alloc f, nint ud);

    [LuaNativeImport]
    public static partial void lua_close(lua_State* L);

    [LuaNativeImport]
    public static partial lua_State* lua_newthread(lua_State* L);

    [LuaNativeImport]
    public static partial lua_CFunction lua_atpanic(lua_State* L, lua_CFunction panicf);

    [LuaNativeImport]
    public static partial int lua_gettop(lua_State* L);

    [LuaNativeImport]
    public static partial void lua_settop(lua_State* L, int idx);

    [LuaNativeImport]
    public static partial void lua_pushvalue(lua_State* L, int idx);

    [LuaNativeImport]
    public static partial void lua_remove(lua_State* L, int idx);

    [LuaNativeImport]
    public static partial void lua_insert(lua_State* L, int idx);

    [LuaNativeImport]
    public static partial void lua_replace(lua_State* L, int idx);

    [LuaNativeImport]
    public static partial int lua_checkstack(lua_State* L, int sz);

    [LuaNativeImport]
    public static partial void lua_xmove(lua_State* from, lua_State* to, int n);

    [LuaNativeImport]
    public static partial int lua_isnumber(lua_State* L, int idx);

    [LuaNativeImport]
    public static partial int lua_isstring(lua_State* L, int idx);

    [LuaNativeImport]
    public static partial int lua_iscfunction(lua_State* L, int idx);

    [LuaNativeImport]
    public static partial int lua_isuserdata(lua_State* L, int idx);

    [LuaNativeImport]
    public static partial int lua_type(lua_State* L, int idx);

    [LuaNativeImport]
    public static partial nint lua_typename(lua_State* L, int tp);
    public static string? Slua_typename(lua_State* L, int tp)
    {
        return Marshal.PtrToStringAnsi(lua_typename(L, tp));
    }

    [LuaNativeImport]
    public static partial int lua_equal(lua_State* L, int idx1, int idx2);

    [LuaNativeImport]
    public static partial int lua_rawequal(lua_State* L, int idx1, int idx2);

    [LuaNativeImport]
    public static partial int lua_lessthan(lua_State* L, int idx1, int idx2);

    [LuaNativeImport]
    public static partial double lua_tonumber(lua_State* L, int idx);

    [LuaNativeImport]
    public static partial nint lua_tointeger(lua_State* L, int idx);

    [LuaNativeImport]
    public static partial int lua_toboolean(lua_State* L, int idx);

    [LuaNativeImport]
    public static partial nint lua_tolstring(lua_State* L, int idx, ref nuint len);
    public static string? Slua_tolstring(lua_State* L, int idx, ref nuint len)
    {
        return Marshal.PtrToStringAnsi(lua_tolstring(L, idx, ref len));
    }

    [LuaNativeImport]
    public static partial nuint lua_objlen(lua_State* L, int idx);

    [LuaNativeImport]
    public static partial lua_CFunction lua_tocfunction(lua_State* L, int idx);

    [LuaNativeImport]
    public static partial nint lua_touserdata(lua_State* L, int idx);

    [LuaNativeImport]
    public static partial lua_State* lua_tothread(lua_State* L, int idx);

    [LuaNativeImport]
    public static partial nint lua_topointer(lua_State* L, int idx);

    [LuaNativeImport]
    public static partial void lua_pushnil(lua_State* L);

    [LuaNativeImport]
    public static partial void lua_pushnumber(lua_State* L, double n);

    [LuaNativeImport]
    public static partial void lua_pushinteger(lua_State* L, nint n);

    [DllImport(DllName, CallingConvention = Convention)]
    public static extern void lua_pushlstring(lua_State* L, string s, nint len);

    [DllImport(DllName, CallingConvention = Convention, CharSet = CharSet.Ansi)]
    public static extern void lua_pushstring(lua_State* L, string s);

    // TODO:
    // [LuaNativeImport]
    // public static partial nint lua_pushvfstring(lua_State* L, string fmt, va_list argp);

    // TODO:
    // [DllImport(DllName, CallingConvention = Convention, EntryPoint = "lua_pushfstring")]
    // public static partial nint _lua_pushfstring(lua_State* L, string fmt, params string[] args);
    // public static string? lua_pushfstring(lua_State* L, string fmt, params string[] args)
    // {
    // 	return Marshal.PtrToStringAnsi(_lua_pushfstring(L, fmt, args));
    // }

    [LuaNativeImport]
    public static partial void lua_pushcclosure(lua_State* L, nint fn, int n);

    [LuaNativeImport]
    public static partial void lua_pushboolean(lua_State* L, int b);

    [LuaNativeImport]
    public static partial void lua_pushlightuserdata(lua_State* L, nint p);

    [LuaNativeImport]
    public static partial int lua_pushthread(lua_State* L);

    [LuaNativeImport]
    public static partial void lua_gettable(lua_State* L, int idx);

    [DllImport(DllName, CallingConvention = Convention)]
    public static extern void lua_getfield(lua_State* L, int idx, string k);

    [LuaNativeImport]
    public static partial void lua_rawget(lua_State* L, int idx);

    [LuaNativeImport]
    public static partial void lua_rawgeti(lua_State* L, int idx, int n);

    [LuaNativeImport]
    public static partial void lua_createtable(lua_State* L, int narr, int nrec);

    [LuaNativeImport]
    public static partial nint lua_newuserdata(lua_State* L, nuint sz);

    [LuaNativeImport]
    public static partial int lua_getmetatable(lua_State* L, int objindex);

    [LuaNativeImport]
    public static partial void lua_getfenv(lua_State* L, int idx);

    [LuaNativeImport]
    public static partial void lua_settable(lua_State* L, int idx);

    [DllImport(DllName, CallingConvention = Convention)]
    public static extern void lua_setfield(lua_State* L, int idx, string k);

    [LuaNativeImport]
    public static partial void lua_rawset(lua_State* L, int idx);

    [LuaNativeImport]
    public static partial void lua_rawseti(lua_State* L, int idx, int n);

    [LuaNativeImport]
    public static partial int lua_setmetatable(lua_State* L, int objindex);

    [LuaNativeImport]
    public static partial int lua_setfenv(lua_State* L, int idx);

    [LuaNativeImport]
    public static partial void lua_call(lua_State* L, int nargs, int nresults);

    [LuaNativeImport]
    public static partial int lua_pcall(lua_State* L, int nargs, int nresults, int errfunc);

    [LuaNativeImport]
    public static partial int lua_cpcall(lua_State* L, lua_CFunction func, nint ud);

    [DllImport(DllName, CallingConvention = Convention)]
    public static extern int lua_load(lua_State* L, lua_Reader reader, nint dt, string chunkname);

    [LuaNativeImport]
    public static partial int lua_dump(lua_State* L, lua_Writer writer, nint data);

    [LuaNativeImport]
    public static partial int lua_yield(lua_State* L, int nresults);

    [LuaNativeImport]
    public static partial int lua_resume(lua_State* L, int narg);

    [LuaNativeImport]
    public static partial int lua_status(lua_State* L);

    public const int LUA_GCSTOP = 0;
    public const int LUA_GCRESTART = 1;
    public const int LUA_GCCOLLECT = 2;
    public const int LUA_GCCOUNT = 3;
    public const int LUA_GCCOUNTB = 4;
    public const int LUA_GCSTEP = 5;
    public const int LUA_GCSETPAUSE = 6;
    public const int LUA_GCSETSTEPMUL = 7;
    public const int LUA_GCISRUNNING = 9;

    [LuaNativeImport]
    public static partial int lua_gc(lua_State* L, int what, int data);

    [LuaNativeImport]
    public static partial int lua_error(lua_State* L);

    [LuaNativeImport]
    public static partial int lua_next(lua_State* L, int idx);

    [LuaNativeImport]
    public static partial void lua_concat(lua_State* L, int n);

    [LuaNativeImport]
    public static partial lua_Alloc lua_getallocf(lua_State* L, ref nint ud);

    [LuaNativeImport]
    public static partial void lua_setallocf(lua_State* L, lua_Alloc f, nint ud);

    public static void lua_pop(lua_State* L, int n)
    {
        lua_settop(L, -(n) - 1);
    }

    public static void lua_newtable(lua_State* L)
    {
        lua_createtable(L, 0, 0);
    }

    public static void lua_register(lua_State* L, string n, lua_CFunction f)
    {
        lua_pushcfunction(L, f);
        lua_setglobal(L, n);
    }

    public static void lua_pushcfunction(lua_State* L, lua_CFunction f)
    {
        lua_pushcclosure(L, Marshal.GetFunctionPointerForDelegate(f), 0);
    }

    public static nuint lua_strlen(lua_State* L, int i)
    {
        return lua_objlen(L, i);
    }

    public static int lua_isfunction(lua_State* L, int n)
    {
        return (lua_type(L, n) == LUA_TFUNCTION) ? 1 : 0;
    }

    public static int lua_istable(lua_State* L, int n)
    {
        return (lua_type(L, n) == LUA_TTABLE) ? 1 : 0;
    }

    public static int lua_islightuserdata(lua_State* L, int n)
    {
        return (lua_type(L, n) == LUA_TLIGHTUSERDATA) ? 1 : 0;
    }

    public static int lua_isnil(lua_State* L, int n)
    {
        return (lua_type(L, n) == LUA_TNIL) ? 1 : 0;
    }

    public static int lua_isboolean(lua_State* L, int n)
    {
        return (lua_type(L, n) == LUA_TBOOLEAN) ? 1 : 0;
    }

    public static int lua_isthread(lua_State* L, int n)
    {
        return (lua_type(L, n) == LUA_TTHREAD) ? 1 : 0;
    }

    public static int lua_isnone(lua_State* L, int n)
    {
        return (lua_type(L, n) == LUA_TNONE) ? 1 : 0;
    }

    public static int lua_isnoneornil(lua_State* L, int n)
    {
        return (lua_type(L, n) <= 0) ? 1 : 0;
    }

    public static void lua_pushliteral(lua_State* L, string s)
    {
        lua_pushlstring(L, s, (nint)s.Length);
    }

    public static void lua_setglobal(lua_State* L, string s)
    {
        lua_setfield(L, LUA_GLOBALSINDEX, s);
    }

    public static void lua_getglobal(lua_State* L, string s)
    {
        lua_getfield(L, LUA_GLOBALSINDEX, s);
    }

    public static string? lua_tostring(lua_State* L, int i)
    {
        nuint temp = 0; // NOP
        return Slua_tolstring(L, i, ref temp);
    }

    public static lua_State* lua_open()
    {
        return luaL_newstate();
    }

    public static void lua_getregistry(lua_State* L)
    {
        lua_pushvalue(L, LUA_REGISTRYINDEX);
    }

    public static int lua_getgccount(lua_State* L)
    {
        return lua_gc(L, LUA_GCCOUNT, 0);
    }

    [LuaNativeImport]
    public static partial void lua_setlevel(lua_State* from, lua_State* to);

    public const int LUA_HOOKCALL = 0;
    public const int LUA_HOOKRET = 1;
    public const int LUA_HOOKLINE = 2;
    public const int LUA_HOOKCOUNT = 3;
    public const int LUA_HOOKTAILRET = 4;

    public const int LUA_MASKCALL = (1 << LUA_HOOKCALL);
    public const int LUA_MASKRET = (1 << LUA_HOOKRET);
    public const int LUA_MASKLINE = (1 << LUA_HOOKLINE);
    public const int LUA_MASKCOUNT = (1 << LUA_HOOKCOUNT);

    [LuaNativeImport]
    public static partial int lua_getstack(lua_State* L, int level, lua_Debug ar);

    [LuaNativeImport]
    public static partial int lua_getinfo(lua_State* L, string what, lua_Debug ar);

    [LuaNativeImport]
    public static partial nint lua_getlocal(lua_State* L, lua_Debug ar, int n);
    public static string? Slua_getlocal(lua_State* L, lua_Debug ar, int n)
    {
        return Marshal.PtrToStringAnsi(lua_getlocal(L, ar, n));
    }

    [LuaNativeImport]
    public static partial nint lua_setlocal(lua_State* L, lua_Debug ar, int n);
    public static string? Slua_setlocal(lua_State* L, lua_Debug ar, int n)
    {
        return Marshal.PtrToStringAnsi(lua_setlocal(L, ar, n));
    }

    [LuaNativeImport]
    public static partial nint lua_getupvalue(lua_State* L, int funcindex, int n);
    public static string? Slua_getupvalue(lua_State* L, int funcindex, int n)
    {
        return Marshal.PtrToStringAnsi(lua_getupvalue(L, funcindex, n));
    }

    [LuaNativeImport]
    public static partial nint lua_setupvalue(lua_State* L, int funcindex, int n);
    public static string? Slua_setupvalue(lua_State* L, int funcindex, int n)
    {
        return Marshal.PtrToStringAnsi(lua_setupvalue(L, funcindex, n));
    }

    [LuaNativeImport]
    public static partial int lua_sethook(lua_State* L, lua_Hook func, int mask, int count);

    [LuaNativeImport]
    public static partial lua_Hook lua_gethook(lua_State* L);

    [LuaNativeImport]
    public static partial int lua_gethookmask(lua_State* L);

    [LuaNativeImport]
    public static partial int lua_gethookcount(lua_State* L);

    [LuaNativeImport]
    public static partial nint lua_upvalueid(lua_State* L, int idx, int n);

    [LuaNativeImport]
    public static partial void lua_upvaluejoin(lua_State* L, int idx1, int n1, int idx2, int n2);

    [DllImport(DllName, CallingConvention = Convention)]
    public static extern int lua_loadx(lua_State* L, lua_Reader reader, nint dt, string chunkname, string? mode);

    [LuaNativeImport]
    public static partial nint lua_version(lua_State* L);
    public static double Dlua_version(lua_State* L)
    {
        nint mem = lua_version(L);
        if (mem == nint.Zero)
            return 0;
        byte[] arr = new byte[8];
        for (int i = 0; i < arr.Length; i++)
            arr[i] = Marshal.ReadByte(mem, i);
        return BitConverter.ToDouble(arr, 0);
    }

    [LuaNativeImport]
    public static partial void lua_copy(lua_State* L, int fromidx, int toidx);

    [LuaNativeImport]
    public static partial double lua_tonumberx(lua_State* L, int idx, ref int isnum);

    [LuaNativeImport]
    public static partial nint lua_tointegerx(lua_State* L, int idx, ref int isnum);

    [LuaNativeImport]
    public static partial int lua_isyieldable(lua_State* L);

    public const int LUA_ERRFILE = LUA_ERRERR + 1;

    [DllImport(DllName, CallingConvention = Convention)]
    public static extern void luaL_openlib(lua_State* L, string libname, luaL_Reg l, int nup);

    [DllImport(DllName, CallingConvention = Convention)]
    public static extern void luaL_register(lua_State* L, string libname, luaL_Reg l);

    [DllImport(DllName, CallingConvention = Convention)]
    public static extern int luaL_getmetafield(lua_State* L, int obj, string e);

    [DllImport(DllName, CallingConvention = Convention)]
    public static extern int luaL_callmeta(lua_State* L, int obj, string e);

    [DllImport(DllName, CallingConvention = Convention)]
    public static extern int luaL_typerror(lua_State* L, int narg, string tname);

    [DllImport(DllName, CallingConvention = Convention)]
    public static extern int luaL_argerror(lua_State* L, int numarg, string extramsg);

    [LuaNativeImport]
    public static partial nint luaL_checklstring(lua_State* L, int numArg, ref nint l);
    public static string? SluaL_checklstring(lua_State* L, int numArg, ref nint l)
    {
        return Marshal.PtrToStringAnsi(luaL_checklstring(L, numArg, ref l));
    }

    [DllImport(DllName, CallingConvention = Convention)]
    public static extern nint luaL_optlstring(lua_State* L, int numArg, string def, ref nint l);
    public static string? SluaL_optlstring(lua_State* L, int numArg, string def, ref nint l)
    {
        return Marshal.PtrToStringAnsi(luaL_optlstring(L, numArg, def, ref l));
    }

    [LuaNativeImport]
    public static partial double luaL_checknumber(lua_State* L, int numArg);

    [LuaNativeImport]
    public static partial double luaL_optnumber(lua_State* L, int nArg, double def);

    [LuaNativeImport]
    public static partial nint luaL_checkinteger(lua_State* L, int numArg);

    [LuaNativeImport]
    public static partial nint luaL_optinteger(lua_State* L, int nArg, nint def);

    [LuaNativeImport]
    public static partial void luaL_checkstack(lua_State* L, int sz, string msg);

    [LuaNativeImport]
    public static partial void luaL_checktype(lua_State* L, int narg, int t);

    [LuaNativeImport]
    public static partial void luaL_checkany(lua_State* L, int narg);

    [DllImport(DllName, CallingConvention = Convention)]
    public static extern int luaL_newmetatable(lua_State* L, string tname);

    [DllImport(DllName, CallingConvention = Convention)]
    public static extern nint luaL_checkudata(lua_State* L, int ud, string tname);

    [LuaNativeImport]
    public static partial void luaL_where(lua_State* L, int lvl);

    // TODO: I dont think params works
    //[LuaNativeImport]
    //public static partial int luaL_error(lua_State* L, string fmt, params string[] args);

    [DllImport(DllName, CallingConvention = Convention)]
    public static extern int luaL_checkoption(lua_State* L, int narg, string def, string[] lst);

    public const int LUA_NOREF = -2;
    public const int LUA_REFNIL = -1;

    [LuaNativeImport]
    public static partial int luaL_ref(lua_State* L, int t);

    [LuaNativeImport]
    public static partial void luaL_unref(lua_State* L, int t, int _ref);

    [DllImport(DllName, CallingConvention = Convention)]
    public static extern int luaL_loadfile(lua_State* L, string filename);

    [DllImport(DllName, CallingConvention = Convention)]
    public static extern int luaL_loadbuffer(lua_State* L, string buff, nint sz, string name);

    [DllImport(DllName, CallingConvention = Convention)]
    public static extern int luaL_loadstring(lua_State* L, string s);

    [LuaNativeImport]
    public static partial lua_State* luaL_newstate();

    [DllImport(DllName, CallingConvention = Convention)]
    public static extern nint luaL_gsub(lua_State* L, string s, string p, string r);
    public static string? SluaL_gsub(lua_State* L, string s, string p, string r)
    {
        return Marshal.PtrToStringAnsi(luaL_gsub(L, s, p, r));
    }

    [DllImport(DllName, CallingConvention = Convention)]
    public static extern nint luaL_findtable(lua_State* L, int idx, string fname, int szhint);
    public static string? SluaL_findtable(lua_State* L, int idx, string fname, int szhint)
    {
        return Marshal.PtrToStringAnsi(luaL_findtable(L, idx, fname, szhint));
    }

    [DllImport(DllName, CallingConvention = Convention)]
    public static extern int luaL_fileresult(lua_State* L, int stat, string fname);

    [LuaNativeImport]
    public static partial int luaL_execresult(lua_State* L, int stat);

    [DllImport(DllName, CallingConvention = Convention)]
    public static extern int luaL_loadfilex(lua_State* L, string filename, string? mode);

    [DllImport(DllName, CallingConvention = Convention)]
    public static extern int luaL_loadbufferx(lua_State* L, string buff, nint sz, string name, string? mode);

    [DllImport(DllName, CallingConvention = Convention)]
    public static extern void luaL_traceback(lua_State* L, lua_State* L1, string msg, int level);

    [DllImport(DllName, CallingConvention = Convention)]
    public static extern void luaL_setfuncs(lua_State* L, luaL_Reg[] l, int nup);

    [DllImport(DllName, CallingConvention = Convention)]
    public static extern void luaL_pushmodule(lua_State* L, string modename, int sizehint);

    [DllImport(DllName, CallingConvention = Convention)]
    public static extern nint luaL_testudata(lua_State* L, int ud, string tname);

    [DllImport(DllName, CallingConvention = Convention)]
    public static extern void luaL_setmetatable(lua_State* L, string tname);

    public static void luaL_argcheck(lua_State* L, bool cond, int numarg, string extramsg)
    {
        if (cond == false)
            _ = luaL_argerror(L, numarg, extramsg);
    }

    public static string? luaL_checkstring(lua_State* L, int n)
    {
        nint temp = 0; // NOP
        return SluaL_checklstring(L, n, ref temp);
    }

    public static string? luaL_optstring(lua_State* L, int n, string d)
    {
        nint temp = 0; // NOP
        return SluaL_optlstring(L, n, d, ref temp);
    }

    public static int luaL_checkint(lua_State* L, int n)
    {
        return (int)luaL_checkinteger(L, n);
    }

    public static int luaL_optint(lua_State* L, int n, nint d)
    {
        return (int)luaL_optinteger(L, n, d);
    }

    public static nint luaL_checknint(lua_State* L, int n)
    {
        return (nint)luaL_checkinteger(L, n);
    }

    public static nint luaL_optnint(lua_State* L, int n, nint d)
    {
        return (nint)luaL_optinteger(L, n, d);
    }

    public static string? luaL_typename(lua_State* L, int i)
    {
        return Slua_typename(L, lua_type(L, i));
    }

    public static int luaL_dofile(lua_State* L, string fn)
    {
        int status = luaL_loadfile(L, fn);
        if (status > 0)
            return status;
        return lua_pcall(L, 0, LUA_MULTRET, 0);
    }

    public static int luaL_dostring(lua_State* L, string s)
    {
        int status = luaL_loadstring(L, s);
        if (status > 0)
            return status;
        return lua_pcall(L, 0, LUA_MULTRET, 0);
    }

    public static void luaL_getmetatable(lua_State* L, string n)
    {
        lua_getfield(L, LUA_REGISTRYINDEX, n);
    }

    public delegate T luaL_Function<T>(lua_State* L, int n);

    public static T luaL_opt<T>(lua_State* L, luaL_Function<T> f, int n, T d)
    {
        return lua_isnoneornil(L, n) > 0 ? d : f(L, n);
    }

    public static void luaL_newlibtable(lua_State* L, luaL_Reg[] l)
    {
        lua_createtable(L, 0, l.Length - 1);
    }

    public static void luaL_newlib(lua_State* L, luaL_Reg[] l)
    {
        luaL_newlibtable(L, l);
        luaL_setfuncs(L, l, 0);
    }

    public static void luaL_addchar(luaL_Buffer B, byte c)
    {
        if ((nint)B.p >= B.buffer.Length + LUAL_BUFFERSIZE)
            luaL_prepbuffer(B);
        Marshal.WriteByte((nint)B.p, c);
        B.p += 1;
    }

    public static void luaL_putchar(luaL_Buffer B, byte c)
    {
        luaL_addchar(B, c);
    }

    public static void luaL_addsize(luaL_Buffer B, int n)
    {
        B.p += n;
    }

    [LuaNativeImport]
    public static partial void luaL_buffinit(lua_State* L, luaL_Buffer B);

    [LuaNativeImport]
    public static partial char* luaL_prepbuffer(luaL_Buffer B);

    [DllImport(DllName, CallingConvention = Convention)]
    public static extern void luaL_addlstring(luaL_Buffer B, string s, nint l);

    [DllImport(DllName, CallingConvention = Convention)]
    public static extern void luaL_addstring(luaL_Buffer B, string s);

    [LuaNativeImport]
    public static partial void luaL_addvalue(luaL_Buffer B);

    [LuaNativeImport]
    public static partial void luaL_pushresult(luaL_Buffer B);

    public const string LUA_FILEHANDLE = "FILE*";

    public const string LUA_COLIBNAME = "coroutine";
    public const string LUA_MATHLIBNAME = "math";
    public const string LUA_STRLIBNAME = "string";
    public const string LUA_TABLIBNAME = "table";
    public const string IOLIBNAME = "io";
    public const string OSLIBNAME = "os";
    public const string LOADLIBNAME = "package";
    public const string DBLIBNAME = "debug";
    public const string BITLIBNAME = "bit";
    public const string JITLIBNAME = "jit";
    public const string FFILIBNAME = "fii";

    [LuaNativeImport]
    public static partial int luaopen_base(lua_State* L);

    [LuaNativeImport]
    public static partial int luaopen_math(lua_State* L);

    [LuaNativeImport]
    public static partial int luaopen_string(lua_State* L);

    [LuaNativeImport]
    public static partial int luaopen_table(lua_State* L);

    [LuaNativeImport]
    public static partial int luaopen_io(lua_State* L);

    [LuaNativeImport]
    public static partial int luaopen_os(lua_State* L);

    [LuaNativeImport]
    public static partial int luaopen_package(lua_State* L);

    [LuaNativeImport]
    public static partial int luaopen_debug(lua_State* L);

    [LuaNativeImport]
    public static partial int luaopen_bit(lua_State* L);

    [LuaNativeImport]
    public static partial int luaopen_jit(lua_State* L);

    [LuaNativeImport]
    public static partial int luaopen_ffi(lua_State* L);

    [DllImport(DllName, CallingConvention = Convention)]
    public static extern int luaopen_string_buffer(lua_State* L);

    [LuaNativeImport]
    public static partial void luaL_openlibs(lua_State* L);
}
#pragma warning restore SYSLIB1054
#pragma warning restore IDE1006
#pragma warning restore CA1401
