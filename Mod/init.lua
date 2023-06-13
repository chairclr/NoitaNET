local ffi = require("ffi")

ffi.cdef([[

void* LoadLibraryA(const char*);

void RegisterActiveMods(const char**, int);

]])

local dll_path = "mods/NoitaNET/NoitaNET.NativeLoader.dll"

assert(ffi.C.LoadLibraryA(dll_path) ~= nil)

local lib = ffi.load(dll_path)

local modIds = ModGetActiveModIDs()
local modCount = #modIds;

local modFolders = ffi.new("const char*[" .. modCount .. "]", modIds);

lib.RegisterActiveMods(modFolders, modCount);