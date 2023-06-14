## Creating a mod Assembly

Mod assemblies must:
 - Be class libraries
 - Target .NET 7.0
 - Reference [NoitaNET.API](/NoitaNET.API/) (same version that [NoitaNET.Loader](/NoitaNET.Loader/) targets)

## Letting NoitaNET know about your mod

NoitaNET mods are placed in your Noita mods folder (likely in `C:\Program Files (x86)\Steam\steamapps\common\Noita\mods`), just like other Noita mods

Your mod must include a `mod.xml` file and your mod assembly

## IMPORTANT

To let NoitaNET know where your mod assembly is located and what it is named, you must add the `noitanet_assembly_path` attribute to your `<Mod />` element

The `noitanet_assembly_path` attribute will tell `NoitaNET.Loader` which assembly it should load

See [NoitaNET.TestMod](/NoitaNET.TestMod/) for an example of this

## Mod entry point

Your mod entry point class must:
 - Be public
 - Inhereit from `NoitaNET.API.Mod`
 - Have a [NoitaNET.API.ModEntryAttribute]
 - Be the only class in the assembly that matches the above conditions

Example mod entry point:

```
using NoitaNET.API;

[ModEntry]
public class TestMod : Mod
{
    public TestMod(string name, string description)
        : base(name, description)
    {

    }
}
```
