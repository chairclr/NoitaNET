using System.Xml;
using NoitaNET.Loader.Logging;
using NoitaNET.Loader.Utility;

namespace NoitaNET.Loader.Services;

internal class ModFinderService
{
    public static List<ModDescription> FindMods(string[] activeMods)
    {
        List<ModDescription> mods = new List<ModDescription>();

        foreach (string mod in activeMods)
        {
            string modFolderPath = Path.Combine(PathService.NoitaModsFolder, mod);

            // It's a steam workshop mod, most likely, and doesn't exist in the regular Noita mods folder
            if (!Directory.Exists(modFolderPath))
            {
                Logger.Instance.LogDebug($"Ignoring {mod}; most likely a steam workshop mod");
                continue;
            }

            string modXMLPath = Path.Combine(modFolderPath, "mod.xml");

            NoitaModDescriptionXml modInfo;

            // Big try-catch block for some error handling
            try
            {
                using FileStream fs = new FileStream(modXMLPath, FileMode.Open);
                using XmlReader xmlReader = XmlReader.Create(fs);

                modInfo = XmlUtility.Deserialize<NoitaModDescriptionXml>(xmlReader)!;
            }
            catch (FileNotFoundException fnfe)
            {
                Logger.Instance.LogDebug($"Ignoring {mod}; does not have a mod.xml file.\n{fnfe}");
                continue;
            }
            catch (DirectoryNotFoundException dnfe)
            {
                Logger.Instance.LogDebug($"Ignoring {mod}; does not have a mod.xml file.\n{dnfe}");
                continue;
            }
            catch (InvalidOperationException ioe)
            {
                Logger.Instance.LogDebug($"Ignoring {mod}; does not have a valid mod.xml file.\n{ioe}");
                continue;
            }

            if (modInfo.AssemblyPath is null)
            {
                Logger.Instance.LogDebug($"Ignoring {mod}; does not contain a noitanet_assembly_path property");
                continue;
            }

            modInfo.AssemblyPath = Path.Combine(modFolderPath, modInfo.AssemblyPath);

            if (!File.Exists(modInfo.AssemblyPath))
            {
                Logger.Instance.LogDebug($"Ignoring {mod}; provided assembly file does not exist. {modInfo.AssemblyPath}");
                continue;
            }

            Logger.Instance.LogInformation($"Got mod dll path: {modInfo.AssemblyPath}");

            mods.Add(new ModDescription(modInfo));
        }

        return mods;
    }
}
