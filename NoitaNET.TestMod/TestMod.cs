using NoitaNET.API;
using NoitaNET.API.Logging;
using NoitaNET.API.Lua;

namespace NoitaNET.TestMod;

[ModEntry]
public class TestMod : Mod
{
    public TestMod(string name, string description)
        : base(name, description)
    {
        Logger.Instance.LogInformation($"Hello from {name}");
    }

    public override unsafe void OnWorldPostUpdate()
    {
        //Logger.Instance.LogInformation($"TestMod::OnWorldPostUpdate");

        //string code =
        //    """
        //    local x, y = EntityGetTransform(EntityGetWithTag("player_unit")[1])
        //    CreateItemActionEntity("ADD_TRIGGER", x, y + 32)
        //    """;

        //LuaNative.luaL_dostring(LuaState, code);
    }

    public override void OnWorldPreUpdate()
    {
        //Logger.Instance.LogInformation($"TestMod::OnWorldPreUpdate");
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
