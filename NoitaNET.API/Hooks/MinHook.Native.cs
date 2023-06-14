using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace NoitaNET.API.Hooks;

// Unsafe MinHook API
// The MinHook is compiled into the NativeLoader for funsies
internal unsafe partial class MinHook
{
    public enum MH_Status : int
    {
        // Unknown error. Should not be returned.
        MH_UNKNOWN = -1,

        // Successful.
        MH_OK = 0,

        // MinHook is already initialized.
        MH_ERROR_ALREADY_INITIALIZED,

        // MinHook is not initialized yet, or already uninitialized.
        MH_ERROR_NOT_INITIALIZED,

        // The hook for the specified target function is already created.
        MH_ERROR_ALREADY_CREATED,

        // The hook for the specified target function is not created yet.
        MH_ERROR_NOT_CREATED,

        // The hook for the specified target function is already enabled.
        MH_ERROR_ENABLED,

        // The hook for the specified target function is not enabled yet, or already
        // disabled.
        MH_ERROR_DISABLED,

        // The specified pointer is invalid. It points the address of non-allocated
        // and/or non-executable region.
        MH_ERROR_NOT_EXECUTABLE,

        // The specified target function cannot be hooked.
        MH_ERROR_UNSUPPORTED_FUNCTION,

        // Failed to allocate memory.
        MH_ERROR_MEMORY_ALLOC,

        // Failed to change the memory protection.
        MH_ERROR_MEMORY_PROTECT,

        // The specified module is not loaded.
        MH_ERROR_MODULE_NOT_FOUND,

        // The specified function is not found.
        MH_ERROR_FUNCTION_NOT_FOUND
    }

    [LibraryImport("NoitaNET.NativeLoader")]
    [UnmanagedCallConv(CallConvs = new Type[] { typeof(CallConvStdcall) })]
    public static partial MH_Status MH_CreateHook(nint pTarget, nint pDetour, nint* ppOriginal);

    [LibraryImport("NoitaNET.NativeLoader")]
    [UnmanagedCallConv(CallConvs = new Type[] { typeof(CallConvStdcall) })]
    public static partial MH_Status MH_RemoveHook(nint pTarget);

    [LibraryImport("NoitaNET.NativeLoader")]
    [UnmanagedCallConv(CallConvs = new Type[] { typeof(CallConvStdcall) })]
    public static partial MH_Status MH_EnableHook(nint pTarget);

    [LibraryImport("NoitaNET.NativeLoader")]
    [UnmanagedCallConv(CallConvs = new Type[] { typeof(CallConvStdcall) })]
    public static partial MH_Status MH_DisableHook(nint pTarget);
}
