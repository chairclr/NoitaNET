using NoitaNET.API.Logging;

namespace NoitaNET.API.Hooks;

/// <summary>
/// Represents a hook at the assembly level
/// </summary>
public class NativeHook : IDisposable
{
    private static HashSet<NativeHook> Pinned = new HashSet<NativeHook>();

    /// <summary>
    /// Target function to hook
    /// </summary>
    public readonly nint Target;

    /// <summary>
    /// Function that execution will be redirected to
    /// </summary>
    public readonly nint Detour;

    /// <summary>
    /// Function in order to call the original function
    /// </summary>
    public readonly nint Original;

    private bool InternalEnabled = false;

    /// <summary>
    /// Set to true or false to enable or disable the hook. 
    /// </summary>
    public bool Enabled
    {
        get => InternalEnabled;
        set
        {
            if (Disposed) throw new ObjectDisposedException(nameof(NativeHook));

            Logger.Instance.LogDebug($"Enabaling NativeHook from 0x{Target} to {Detour}");

            MinHook.MH_Status status = MinHook.MH_EnableHook(Target);

            if (status != MinHook.MH_Status.MH_OK)
            {
                Logger.Instance.LogError($"Failed to enable NativeHook from 0x{Target} to {Detour}: {status}");
            }
            else
            {
                InternalEnabled = value;
            }
        }
    }

    private readonly bool WasCreated = false;

    private bool Disposed;

    /// <summary>
    /// Creates a hook from <paramref name="target"/> that detours to <paramref name="detour"/>
    /// </summary>
    /// <param name="target">The function to hook</param>
    /// <param name="detour">The. Just the.</param>
    /// <param name="enable">Whether or not to automatically enable the hook after creation</param>
    /// <remarks>
    /// Automatically enables the hook after creation by default
    /// </remarks>
    public NativeHook(nint target, nint detour, bool enable = true)
    {
        Target = target;

        Detour = detour;

        Logger.Instance.LogDebug($"Creating NativeHook from 0x{Target} to {Detour}");

        MinHook.MH_Status status;
        unsafe
        {
            fixed (nint* ppOriginal = &Original)
            {
                status = MinHook.MH_CreateHook(Target, Detour, ppOriginal);
            }
        }

        if (status != MinHook.MH_Status.MH_OK)
        {
            Logger.Instance.LogError($"Failed to create NativeHook from 0x{Target} to {Detour}: {status}");
            return;
        }

        WasCreated = true;

        if (enable)
        {
            Enabled = true;
        }
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!Disposed)
        {
            if (WasCreated)
            {
                if (Enabled)
                {
                    MinHook.MH_DisableHook(Target);
                }

                MinHook.MH_RemoveHook(Target);
            }
            Disposed = true;
        }
    }

    /// <summary>
    /// Disables and removes the hook
    /// </summary>
    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
