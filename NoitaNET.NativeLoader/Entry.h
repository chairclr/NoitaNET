#pragma once
#include "framework.h"
#include <string>
#include <vector>
#include "Util.h"
#include "DotNetHost.h"
#include "NativeLog.h"
#include <lua.hpp>
#include <MinHook.h>
#include "EngineAPIFunction.h"

class Entry
{
private:
    static inline bool LoadedDotNet = false;

    static inline HMODULE ModuleHandle = nullptr;

    static inline std::string DllRootDirectory = "";

    static void InternalLoad(HMODULE hMod);

    static inline std::vector<std::string> ActiveNoitaMods = {};

    static inline std::vector<EngineAPIFunction> EngineFunctions = {};

public:
    static void Load(HMODULE hMod);

    static const std::string& GetDllRootDirectory();

    static bool IsDotNetLoaded();

    static void SetActiveNoitaMods(const char** modFolders, int modCount);

    static const std::vector<std::string>& GetActiveNoitaMods();

    static void AddEngineAPIFunction(const char* name, void* functionPointer);

    static const std::vector<EngineAPIFunction>& GetEngineAPIFunctions();
};
