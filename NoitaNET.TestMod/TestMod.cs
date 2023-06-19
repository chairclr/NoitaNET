using NoitaNET.API;
using NoitaNET.API.Logging;

namespace NoitaNET.TestMod;

[ModEntry]
public class TestMod : Mod
{
    private int UpdateCount = 0;
    private const int BenchmarkEveryNFrames = 1;
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

        //Noita.RawAPI.EntityGetWithTag("player_unit", out nint[] tempEntities);

        //if (tempEntities.Length != 1) return;

        //Noita.RawAPI.EntityGetTransform(tempEntities[0], out double x, out double y, out _, out _, out _);

        //Noita.RawAPI.EntityGetInRadius(x, y, 40, out nint[] entitiesInRadius);

        //foreach (nint entity in entitiesInRadius)
        //{
        //    Noita.RawAPI.EntityHasTag(entity, "player_unit", out bool isPlayer);

        //    if (isPlayer)
        //        continue;

        //    Noita.RawAPI.EntityInflictDamage(entity, 1, "slice", "Get sliced by c#", "NORMAL", 0, 0);
        //}

        if (!RunOnce && UpdateCount % BenchmarkEveryNFrames == 0)
        {
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
