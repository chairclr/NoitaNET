using System.Reflection;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;
using NoitaNET.API;
using NoitaNET.API.Logging;
using NoitaNET.API.Lua;
using NoitaNET.Loader.Services;

namespace NoitaNET.Loader;

public unsafe class EntryHandler
{
    public delegate void EntryDelegate(char** activeMods, int activeModsCount);

    /// <summary>
    /// Entry point function for the entire NoitaNET managed side
    /// </summary>
    public static void Entry(char** activeMods, int activeModsCount)
    {
        string[] managedActiveMods = new string[activeModsCount];

        // We can't directly pass a string[] from C++ to C#, so we must pass a char** and then retrieve the strings in C#
        for (int i = 0; i < managedActiveMods.Length; i++)
        {
            // TODO: Confirm that UTF8 is the right encoding when using "Multi-Byte Character Set"
            // Or switch to unicode lol
            managedActiveMods[i] = Marshal.PtrToStringUTF8((nint)activeMods[i])!;
        }

        List<ModDescription> mods = ModFinderService.FindMods(managedActiveMods);

        ModLoadHandler modLoadHandler = new ModLoadHandler(mods);

        modLoadHandler.LoadMods();
    }


    public struct NativeCallbacks
    {
        public delegate* unmanaged[Cdecl]<void> OnWorldPostUpdate;

        public delegate* unmanaged[Cdecl]<void> OnWorldPreUpdate;

        public delegate* unmanaged[Cdecl]<void> OnModPreInit;

        public delegate* unmanaged[Cdecl]<void> OnModInit;

        public delegate* unmanaged[Cdecl]<void> OnModPostInit;
    }

    public delegate void GetCallbackHandlersDelegate(out NativeCallbacks outCallbacks);

    public static void GetCallbackHandlers(out NativeCallbacks outCallbacks)
    {
        outCallbacks = new NativeCallbacks
        {
            OnWorldPostUpdate = &Callbacks.OnWorldPostUpdate,
            OnWorldPreUpdate = &Callbacks.OnWorldPreUpdate,
            OnModPreInit = &Callbacks.OnModPreInit,
            OnModInit = &Callbacks.OnModInit,
            OnModPostInit = &Callbacks.OnModPostInit,
        };
    }


    [StructLayout(LayoutKind.Sequential)]
    public struct EngineAPIFunction
    {
        private readonly char* InternalName;
        public readonly string Name => Marshal.PtrToStringUTF8((nint)InternalName)!;

        public nint FunctionPointer;
    }

    public delegate void RegisterEngineAPIFunctionsDelegate(EngineAPIFunction* pointer, int length);

    public static void RegisterEngineAPIFunctions(EngineAPIFunction* pointer, int length)
    {
        Span<EngineAPIFunction> engineAPIFunctions = new Span<EngineAPIFunction>(pointer, length);

        // Map function name -> function pointer for use in assigning pointers in the function table by name
        Dictionary<string, nint> engineAPIMap = new Dictionary<string, nint>(engineAPIFunctions.Length);

        for (int i = 0; i < engineAPIFunctions.Length; i++)
        {
            engineAPIMap.Add(engineAPIFunctions[i].Name, engineAPIFunctions[i].FunctionPointer);
        }

        // Set the function pointers for the NoitaNET -> Noita lua API
        // They're contained in an internal class, so we have to use reflection to get the type as well
        FieldInfo[] fields = typeof(Noita).Assembly
            .GetType("NoitaNET.API.EngineAPIFunctionTable")!
            .GetFields(BindingFlags.Static | BindingFlags.Public);

        for (int i = 0; i < fields.Length; i++)
        {
            if (engineAPIMap.TryGetValue(fields[i].Name, out nint fn))
            {
                fields[i].SetValue(null,  fn);
            }
        }
    }
}
