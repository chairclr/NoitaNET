using System.Diagnostics;
using NoitaNET.API;
using NoitaNET.API.Logging;

namespace NoitaNET.TestMod;

[ModEntry]
public class TestMod : Mod
{
    private int FC = 0;
    private int Count = 60 * 10;

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
        FC++;
        if (FC > Count)
        {
            const int n = 100_000_000;
            Stopwatch sw = Stopwatch.StartNew();

            double v = 10000;
            for (int i = 0; i < n; i++)
            {
                v = Noita.Random();
            }
            sw.Stop();
            Logger.Instance.LogInformation($"{v}");
            Logger.Instance.LogInformation($"Random() {n} times took {sw.Elapsed.TotalMilliseconds:F4}ms");
        }
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
