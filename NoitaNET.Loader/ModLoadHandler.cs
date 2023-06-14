using System.Reflection;
using System.Runtime.Loader;
using NoitaNET.API;
using NoitaNET.API.Logging;

namespace NoitaNET.Loader;

internal class ModLoadHandler
{
    // Globally Available
    /// <summary>
    /// List of currently loaded mods
    /// </summary>
    public static IReadOnlyList<Mod> Mods = new List<Mod>();

    /// <summary>
    /// List of mods to load, containing their name, description, and path to their C# entry assembly
    /// </summary>
    private readonly List<ModDescription> ModsToLoad;

    /// <summary>
    /// Working list of mods currently loaded.
    /// We keep this private and then set the public <see cref="Mods"/> equal to it after we finish loading mods
    /// </summary>
    private readonly List<Mod> LoadedMods = new List<Mod>(10);

    public ModLoadHandler(List<ModDescription> modsToLoad)
    {
        ModsToLoad = modsToLoad;

        AssemblyLoadContext.Default.Resolving += (x, y) =>
        {
            Assembly? assembly = AppDomain.CurrentDomain.GetAssemblies().Where(x => x.GetName().FullName == y.FullName).FirstOrDefault();

            if (assembly is null)
            {
                Logger.Instance.LogError($"Failed to resolve assembly dependency {y}");
            }

            return assembly;
        };
    }

    public void LoadMods()
    {
        Logger.Instance.LogInformation("Start loading NoitaNET mod assemblies");

        foreach (ModDescription mod in ModsToLoad)
        {
            Logger.Instance.LogInformation($"Loading {mod.Name} ({mod.AssemblyPath})");

            Assembly assembly;

            try
            {
                assembly = AssemblyLoadContext.Default.LoadFromAssemblyPath(mod.AssemblyPath);
            }
            catch (BadImageFormatException bife)
            {
                Logger.Instance.LogError($"Skipping loading {mod.Name}; invalid assembly file\n{bife}");
                continue;
            }
            catch (Exception ex)
            {
                Logger.Instance.LogError($"Skipping loading {mod.Name};\n{ex}");
                continue;
            }

            mod.Version = assembly.GetName().Version ?? new Version(1, 0, 0, 0);

            Logger.Instance.LogInformation($"Loaded {mod.Name} assembly {assembly.GetName()}");

            Type? entryModType = FindValidEntryModType(mod, assembly);

            if (entryModType is null)
            {
                Logger.Instance.LogError($"Assembly {assembly.GetName()} ({mod.Name}) does not contain a valid entry class definition");
                continue;
            }

            CreateEntryMod(entryModType, mod);
        }

        Mods = LoadedMods;
    }

    /// <summary>
    /// Finds the entry point class that inherits from <see cref="Mod"/> and is annotated with a <see cref="ModEntryAttribute"/>
    /// </summary>
    private static Type? FindValidEntryModType(ModDescription mod, Assembly assembly)
    {
        Type[] types = assembly.GetExportedTypes();

        Type? entryModType = null;

        for (int i = 0; i < types.Length; i++)
        {
            Type type = types[i];

            // We can't create an instance of an abstract or generic type with Acitvator
            if (!type.IsAbstract && !type.IsGenericType && !type.IsInterface && type.IsAssignableTo(typeof(Mod)))
            {
                if (type.CustomAttributes.Any(x => x.AttributeType == typeof(ModEntryAttribute)))
                {
                    if (entryModType is not null)
                    {
                        Logger.Instance.LogError($"Assembly {assembly.GetName()} ({mod.Name}) must only contain one mod entry class");

                        entryModType = null;
                        break;
                    }

                    entryModType = type;
                }
            }
        }

        return entryModType;
    }

    /// <summary>
    /// Creates the instance and adds it to <see cref="LoadedMods"/>
    /// </summary>
    private void CreateEntryMod(Type entryModType, ModDescription modDescription)
    {
        Mod entryMod = (Mod)(Activator.CreateInstance(entryModType, modDescription.Name, modDescription.Description)!);

        LoadedMods.Add(entryMod);

        Logger.Instance.LogInformation($"Created mod {entryModType.Name}");
    }
}
