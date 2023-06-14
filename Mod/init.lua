local ffi = require("ffi")

ffi.cdef([[

void* LoadLibraryA(const char*);

void RegisterActiveMods(const char**, int);

]])

-- NoitaNET.NativeLoader
local nativeLoaderDllPath = "mods/NoitaNET/NoitaNET.NativeLoader.dll"

assert(ffi.C.LoadLibraryA(nativeLoaderDllPath) ~= nil)

local lib = ffi.load(nativeLoaderDllPath)

local modIds = ModGetActiveModIDs()
local modCount = #modIds;

local modFolders = ffi.new("const char*[?]", modCount, modIds);

lib.RegisterActiveMods(modFolders, modCount);

function OnWorldPostUpdate()
    
end