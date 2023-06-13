#pragma once
#include "framework.h"
#include <string>
#include <vector>
#include "Util.h"
#include "DotNetHost.h"
#include "NativeLog.h"

class Entry
{
private:
    static inline bool LoadedDotNet = false;

    static inline HMODULE ModuleHandle = nullptr;

    static inline std::string DllRootDirectory = "";

    static void InternalLoad(HMODULE hMod);

    static inline std::vector<std::string> LoadedNoitaMods = {};

public:
    static void Load(HMODULE hMod);

    static const std::string& GetDllRootDirectory();

    static bool IsDotNetLoaded();

    static void SetLoadedNoitaMods(const char** modFolders, int modCount);
};