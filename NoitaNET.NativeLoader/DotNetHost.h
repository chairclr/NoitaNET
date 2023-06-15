#pragma once
#include "framework.h"
#include "Entry.h"
#include "Callbacks.h"
#include "EngineAPIFunction.h"

#include <map>

// nethost core files
#include <nethost.h>
#include <coreclr_delegates.h>
#include <hostfxr.h>

// for com errors?
#include <comdef.h>

// Should have the same definition as the delegate specified when loading the assembly
typedef void (CORECLR_DELEGATE_CALLTYPE* EntryDelegate)(const char** activeMods, int activeModsCount);

typedef void (CORECLR_DELEGATE_CALLTYPE* GetCallbackHandlersDelegate)(const Callbacks* outCallbacks);

typedef void (CORECLR_DELEGATE_CALLTYPE* RegisterEngineAPIFunctionsDelegate)(const EngineAPIFunction* engineFunctions, int count);

class DotNetHost
{
public:
    static bool LoadHost();

    static Callbacks GetCallbacks();

private:
    static inline hostfxr_initialize_for_runtime_config_fn HostFXRInitFn = nullptr;
    static inline hostfxr_get_runtime_delegate_fn HostFXRGetRuntimeDelegateFn = nullptr;
    static inline hostfxr_close_fn HostFXRCloseFn = nullptr;
    static inline load_assembly_and_get_function_pointer_fn LoadAssemblyAndGetFunctionPointer = nullptr;

    static bool LoadAndStartManagedAssembly();

    static bool LoadHostFXR();

    static void* GetExport(HMODULE hMod, const char* name);

    static inline Callbacks StoredCallbacks = {};
};
