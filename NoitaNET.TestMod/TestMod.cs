using NoitaNET.API;
using NoitaNET.API.Logging;

namespace NoitaNET.TestMod;

[ModEntry]
public class TestMod : Mod
{
    public TestMod(string name, string description)
        : base(name, description)
    {
        Logger.Instance.LogInformation($"Hello from {name}");
    }

    public override void OnWorldPostUpdate()
    {
        Logger.Instance.LogInformation($"TestMod::OnWorldPostUpdate");
    }

    public override void OnWorldPreUpdate()
    {
        Logger.Instance.LogInformation($"TestMod::OnWorldPreUpdate");
    }

    public override void OnModPreInit()
    {
        Logger.Instance.LogInformation($"TestMod::OnModPreInit");
    }

    public override void OnModInit()
    {
        Logger.Instance.LogInformation($"TestMod::OnModInit");
    }

    public override void OnModPostInit()
    {
        Logger.Instance.LogInformation($"TestMod::OnModPostInit");
    }
}
