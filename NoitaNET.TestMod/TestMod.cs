using NoitaNET.API;
using NoitaNET.API.Logging;

namespace NoitaNET.TestMod;

[ModEntry]
public class TestMod : Mod
{
    public TestMod(string name, string description) : base(name, description)
    {
        Logger.Instance.LogInformation($"Hello from {name}");
    }

    public override void OnWorldPostUpdate()
    {
        Logger.Instance.LogInformation($"TestMod::OnWorldPostUpdate was called!!");
    }
}
