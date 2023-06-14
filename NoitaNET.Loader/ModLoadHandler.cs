using System.Reflection;
using System.Runtime.Loader;
using NoitaNET.API;
using NoitaNET.Loader.Logging;

namespace NoitaNET.Loader;

internal class ModLoadHandler
{
    private readonly List<ModDescription> ModsToLoad;

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

            CreateEntryMod(entryModType);
        }
    }

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

    private void CreateEntryMod(Type entryModType)
    {
        Mod entryMod = (Mod)(Activator.CreateInstance(entryModType)!);

        LoadedMods.Add(entryMod);

        Logger.Instance.LogInformation($"Created mod {entryModType.Name}");
    }
}
