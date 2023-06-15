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
    // https://github.com/LuaJIT/LuaJIT/blob/ff6c496ba1b51ed360065cbc5259f62becd70daa/src/lj_obj.h#L458
    void* func = *(void**)(lf_i + 20);

    Entry::AddEngineAPIFunction(name, func);
}