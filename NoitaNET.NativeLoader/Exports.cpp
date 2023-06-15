#include "Exports.h"

NOITANET_LUA_EXPORT void RegisterActiveMods(const char** modFolders, int folderCount)
{
    Entry::SetActiveNoitaMods(modFolders, folderCount);
}

NOITANET_LUA_EXPORT void WaitForDotnetInit()
{
    while (!Entry::IsDotNetLoaded())
    {
        Sleep(16);
    }
}

NOITANET_LUA_EXPORT Callbacks GetCallbacks()
{
    return DotNetHost::GetCallbacks();
}

NOITANET_LUA_EXPORT void RegisterEngineAPIFunction(const char* name, int lf_i)
{
    void* func = *(void**)(lf_i + 20);

    NativeLog::LogInformation(Util::FormatString("%s: %p", name, func));
}