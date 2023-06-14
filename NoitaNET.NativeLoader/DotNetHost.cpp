#include "DotNetHost.h"

bool DotNetHost::LoadHost()
{
    NativeLog::LogInformation("Loading .NET");

    // Load the hostfxr library and get its exports
    if (!LoadHostFXR())
    {
        NativeLog::LogError("Failed to load hostfxr");
        return false;
    }
    
    std::string runtimeconfigPath = Entry::GetDllRootDirectory() + "\\NoitaNET.Loader\\NoitaNET.Loader.runtimeconfig.json";

    NativeLog::LogInformation(Util::FormatString("Initializing hostfxr with runtimeconfig: %s", runtimeconfigPath.c_str()));

    // Initialize hostfxr with the path to the managed dll runtimeconfig
    hostfxr_handle context = nullptr;
    int rc = HostFXRInitFn(Util::StringToWide(runtimeconfigPath).c_str(), nullptr, &context);

    if (rc != 0 || context == nullptr)
    {
        NativeLog::LogError(Util::FormatString("Failed to initialize hostfxr: %s", _com_error((HRESULT)rc).ErrorMessage()));
        return false;
    }

    // Retrieve the LoadAssemblyAndGetFunctionPointer function from hostfxr
    void* loadAssemblyFn;
    rc = HostFXRGetRuntimeDelegateFn(
        context,
        hdt_load_assembly_and_get_function_pointer,
        &loadAssemblyFn);

    if (rc != 0 || loadAssemblyFn == nullptr)
    {
        NativeLog::LogError("Failed to get LoadAssembly function pointer");
        return false;
    }

    LoadAssemblyAndGetFunctionPointer = (load_assembly_and_get_function_pointer_fn)loadAssemblyFn;

    NativeLog::LogInformation(Util::FormatString("Got LoadAssembly function pointer: %p", LoadAssemblyAndGetFunctionPointer));

    // Close the hostfxr context
    HostFXRCloseFn(context);

    return LoadAndStartManagedAssembly();
}

bool DotNetHost::LoadAndStartManagedAssembly()
{
    std::string managedLibraryPath = Entry::GetDllRootDirectory() + "\\NoitaNET.Loader\\NoitaNET.Loader.dll";

    NativeLog::LogInformation(Util::FormatString("Loading managed assmelby from path: %s", managedLibraryPath.c_str()));

    // Actually load the library and get the entry function
    EntryDelegate entry = NULL;
    int rc = LoadAssemblyAndGetFunctionPointer(
        Util::StringToWide(managedLibraryPath).c_str(),
        // Specify the fully qualified type name and assembly
        L"NoitaNET.Loader.EntryHandler, NoitaNET.Loader",
        // Function to get pointer of inside of the specified type
        L"Entry",
        // Delegate that defines the signature of the entry function
        // This is not required, but it can be useful later when we want to pass parameters or something to the entry function
        L"NoitaNET.Loader.EntryHandler+EntryDelegate, NoitaNET.Loader",
        NULL,
        (void**)&entry);

    if (rc != 0)
    {
        NativeLog::LogError(Util::FormatString("Failed to get Entry function pointer: %s", _com_error((HRESULT)rc).ErrorMessage()));
        return false;
    }

    NativeLog::LogInformation(Util::FormatString("Got Entry function pointer: %p", entry));

    const std::vector<std::string> activeMods = Entry::GetActiveNoitaMods();

    const char** cActiveMods = new const char*[activeMods.size() + 1];

    // Populate the charArray with C-style strings
    for (size_t i = 0; i < activeMods.size(); ++i) {
        cActiveMods[i] = activeMods[i].c_str();
    }

    entry(cActiveMods, activeMods.size());

    delete[] cActiveMods;

    return true;
}

bool DotNetHost::LoadHostFXR()
{
    // Since we keep nethost.dll in the same directory as our dll, we delay the loading of it until here
    // We also tell the linker to delay the loading of nethost.dll in the properties
    HMODULE netHostModule = LoadLibraryA((Entry::GetDllRootDirectory() + "\\nethost.dll").c_str());

    if (!netHostModule)
    {
        NativeLog::LogError("Failed to load nethost.dll from file");
        return false;
    }

    // Get the hostfxr.dll path from nethost.dll
    char_t hostFXRPath[MAX_PATH];
    size_t bufferSize = sizeof(hostFXRPath) / sizeof(char_t);
    int rc = get_hostfxr_path(hostFXRPath, &bufferSize, nullptr);
    if (rc != 0)
    {
        NativeLog::LogError("Failed to get hostfxr.dll path");
        return false;
    }

    NativeLog::LogInformation(Util::FormatString("Got hostfxr path: %s", Util::WideToString(hostFXRPath).c_str()));

    // Load the hostfxr library based on the path we got from nethost.dll
    HMODULE lib = LoadLibraryW(hostFXRPath);

    if (!lib)
    {
        NativeLog::LogError("Failed to load hostfxr.dll from file");
        return false;
    }

    // Get the important exports from hostfxr.dll
    HostFXRInitFn = (hostfxr_initialize_for_runtime_config_fn)GetExport(lib, "hostfxr_initialize_for_runtime_config");
    HostFXRGetRuntimeDelegateFn = (hostfxr_get_runtime_delegate_fn)GetExport(lib, "hostfxr_get_runtime_delegate");
    HostFXRCloseFn = (hostfxr_close_fn)GetExport(lib, "hostfxr_close");

    // Return false if any of these functions is null
    return (HostFXRInitFn && HostFXRGetRuntimeDelegateFn && HostFXRCloseFn);
}

void* DotNetHost::GetExport(HMODULE hMod, const char* name)
{
    return GetProcAddress(hMod, name);
}
