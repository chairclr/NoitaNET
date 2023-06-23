using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using NoitaNET.API;
using NoitaNET.API.Hooks;
using NoitaNET.API.Logging;
using NoitaNET.API.Lua;

namespace NoitaNET.TestMod;

[ModEntry]
public class TestMod : Mod
{
    private int UpdateCount = 0;
    private const int BenchmarkEveryNFrames = 60 * 1;
    private bool RunOnce = false;

    private Benchmarks Benchmarks;

    public TestMod(string name, string description)
        : base(name, description)
    {
        Logger.Instance.LogInformation($"Hello from {name}");

        Benchmarks = new Benchmarks(RawEngineAPI);
    }

    public void X()
    {
        Logger.Instance.LogDebug("yeah");
    }

    public override unsafe void OnWorldPostUpdate()
    {
        UpdateCount++;

        if (!RunOnce)
        {
            //RawEngineAPI.EntityGetWithTag("player_unit", out nint[] tempEntities);

            //if (tempEntities.Length != 1) return;

            //RunOnce = true;

            //nint player = tempEntities[0];

            //RawEngineAPI.EntityAddComponent(player, "LuaComponent", out nint id);

            ////MethodInfo method = typeof(TestMod).GetMethod("X", BindingFlags.Public | BindingFlags.Instance)!;
            //Delegate dx = X;
            //nint x = dx.GetFunctionPointer();

            //byte[] bytes = new byte[16 + 2];

            //bytes[0] = (byte)':';

            //MessageListenerPassthroughData data = new MessageListenerPassthroughData(x, this);

            //data.CopyToString(bytes.AsSpan(1));

            //RawEngineAPI.ComponentSetValue2(id, "script_shot", bytes);
        }

        //RawEngineAPI.EntityGetTransform(tempEntities[0], out double x, out double y, out _, out _, out _);

        //RawEngineAPI.EntityGetInRadius(x, y, 40, out nint[] entitiesInRadius);

        //foreach (nint entity in entitiesInRadius)
        //{
        //    RawEngineAPI.EntityHasTag(entity, "player_unit", out bool isPlayer);

        //    if (isPlayer)
        //        continue;

        //    RawEngineAPI.EntityInflictDamage(entity, 1, "slice", "Test kill", "NORMAL", 0, 0);
        //}

        if (!RunOnce && UpdateCount % BenchmarkEveryNFrames == 0)
        {
            UpdateCount = 0;

            //Benchmarks.BenchmarkPushStringAndPopNew(50_000_000);
            //Benchmarks.BenchmarkPushStringAndPopOld(50_000_000);
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
