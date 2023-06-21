local ffi = require("ffi")

ffi.cdef([[

void* LoadLibraryA(const char*);

void RegisterActiveMods(const char**, int);

void WaitForDotnetInit();

void RegisterEngineAPIFunction(const char*,int);

typedef void (*FOnWorldPostUpdate)();
typedef void (*FOnWorldPreUpdate)();
typedef void (*FOnModPreInit)();
typedef void (*FOnModInit)();
typedef void (*FOnModPostInit)();

typedef struct
{
    FOnWorldPostUpdate OnWorldPostUpdate;
    FOnWorldPreUpdate OnWorldPreUpdate;
    FOnModPreInit OnModPreInit;
    FOnModInit OnModInit;
    FOnModPostInit OnModPostInit;
} Callbacks;

Callbacks GetCallbacks();

]])

-- NoitaNET.NativeLoader
local nativeLoaderDllPath = "mods/NoitaNET/NoitaNET.NativeLoader.dll"

assert(ffi.C.LoadLibraryA(nativeLoaderDllPath) ~= nil)

local lib = ffi.load(nativeLoaderDllPath)

function GetAndRegisterAllMods()
  local modIds = ModGetActiveModIDs()
  local modCount = #modIds

  local modFolders = ffi.new("const char*[?]", modCount, modIds)

  lib.RegisterActiveMods(modFolders, modCount)
end


function GetAndRegisterAllFunctions()

  local function GetAndRegisterFunction(name)
    local func = getfenv()[name]
    if not func then return end
    local addr = tonumber(tostring(func):sub(11))
    lib.RegisterEngineAPIFunction(name, addr)
  end

  local docsFile = io.open("tools_modding/lua_api_documentation.txt", "rt")
  for line in docsFile:lines() do
	local name = line:match("^([^(\n]+)%(")
	if name then
	  GetAndRegisterFunction(name)
	end
  end
end

GetAndRegisterAllMods()

GetAndRegisterAllFunctions()

lib.WaitForDotnetInit()

local callbackTable = lib.GetCallbacks()

function OnWorldPostUpdate()
    callbackTable.OnWorldPostUpdate()
end

function OnWorldPreUpdate()
    callbackTable.OnWorldPreUpdate()
end

function OnModPreInit()
    callbackTable.OnModPreInit();
end

function OnModInit()
    callbackTable.OnModInit();
end

function OnModPostInit()
    callbackTable.OnModPostInit();
end
