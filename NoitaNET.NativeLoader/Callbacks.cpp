#include "Callbacks.h"

extern "C" __declspec(dllexport)
void RegisterActiveMods(const char** modFolders, int folderCount)
{
    Entry::SetActiveNoitaMods(modFolders, folderCount);
}

extern "C" __declspec(dllexport)
void WaitForDotnetInit()
{
    while (!Entry::IsDotNetLoaded())
    {
        Sleep(16);
    }
}