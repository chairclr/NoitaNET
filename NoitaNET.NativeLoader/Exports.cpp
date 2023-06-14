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