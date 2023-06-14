local ffi = require("ffi")

ffi.cdef([[

void* LoadLibraryA(const char*);

void RegisterActiveMods(const char**, int);

void WaitForDotnetInit();

typedef void (*FOnWorldPostUpdate)();
typedef void (*FOnWorldPreUpdate)();

typedef struct
{
    FOnWorldPostUpdate OnWorldPostUpdate;
    FOnWorldPreUpdate OnWorldPreUpdate;
} Callbacks;

Callbacks GetCallbacks();

]])

-- NoitaNET.NativeLoader
local nativeLoaderDllPath = "mods/NoitaNET/NoitaNET.NativeLoader.dll"

assert(ffi.C.LoadLibraryA(nativeLoaderDllPath) ~= nil)

local lib = ffi.load(nativeLoaderDllPath)

local modIds = ModGetActiveModIDs()
local modCount = #modIds

local modFolders = ffi.new("const char*[?]", modCount, modIds)

lib.RegisterActiveMods(modFolders, modCount)

lib.WaitForDotnetInit()

local callbackTable = lib.GetCallbacks()

function OnWorldPostUpdate()
    callbackTable.OnWorldPostUpdate()
end

function OnWorldPreUpdate()
    callbackTable.OnWorldPreUpdate()
end

-- OnWorldPostUpdate = callbackTable.OnWorldPostUpdate
-- OnWorldPreUpdate = callbackTable.OnWorldPreUpdate