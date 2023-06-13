local ffi = require("ffi")

ffi.cdef([[

void* LoadLibraryA(const char*);

]])

local dll_path = "mods/NoitaNET/NoitaNET.NativeLoader.dll"

assert(ffi.C.LoadLibraryA(dll_path) ~= nil)