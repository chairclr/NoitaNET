using System.Xml.Serialization;

namespace NoitaNET.Loader;

/// <summary>
/// Represents the <![CDATA[<Mod />]]> element in mod.xml
/// </summary>
[XmlType("Mod")]
public class NoitaModDescriptionXml
{
    [XmlAttribute("name")]
    public string Name { get; set; } = "";

    [XmlAttribute("description")]
    public string Description { get; set; } = "";

    [XmlAttribute("noitanet_assembly_path")]
    public string? AssemblyPath { get; set; }
}

/// <summary>
/// Represents a mod
/// </summary>
internal class ModDescription
{
    public string Name { get; set; }

    public string Description { get; set; }

    public string AssemblyPath { get; set; }

    public Version Version { get; set; }

    public ModDescription(NoitaModDescriptionXml modXML)
    {
        Name = modXML.Name;

        Description = modXML.Description;

        AssemblyPath = modXML.AssemblyPath!;

        Version = new Version(0, 0, 0, 0);
    }
}
