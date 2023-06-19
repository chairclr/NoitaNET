using NoitaNET.API;
using NoitaNET.API.Logging;

namespace NoitaNET.TestMod;

[ModEntry]
public class TestMod : Mod
{
    private int UpdateCount = 0;
    private const int BenchmarkEveryNFrames = 4 * 60;
    private bool RunOnce = false;

    private Benchmarks Benchmarks;

    public TestMod(string name, string description)
        : base(name, description)
    {
        Logger.Instance.LogInformation($"Hello from {name}");

        Benchmarks = new Benchmarks(Noita);
    }

    public override unsafe void OnWorldPostUpdate()
    {
        UpdateCount++;

        if (!RunOnce && UpdateCount % BenchmarkEveryNFrames == 0)
        {
            RunOnce = true;
            UpdateCount = 0;

            //Noita.CellFactory_GetType("water", out nint waterType);
            //Noita.CellFactory_GetType("magic_liquid_protection_all", out nint amborsiaType);

            //Logger.Instance.LogInformation($"Converting {waterType} to {amborsiaType}");

            //Noita.ConvertMaterialEverywhere(waterType, amborsiaType);

            //Noita.EntityLoad(@"data/entities/animals/boss_fish/fish_giga.xml", 0, -200, out nint return_value_entity_id);

            //Logger.Instance.LogInformation($"ID: {return_value_entity_id}");
        }
    }

    public override void OnWorldPreUpdate()
    {

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
