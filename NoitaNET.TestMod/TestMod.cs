using System.Diagnostics;
using System.Numerics;
using NoitaNET.API;
using NoitaNET.API.Logging;

namespace NoitaNET.TestMod;

[ModEntry]
public class TestMod : Mod
{
    private int UpdateCount = 0;
    private const int BenchmarkEveryNFrames = 4 * 60;

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

        if (UpdateCount % BenchmarkEveryNFrames == 0)
        {
            UpdateCount = 0;

            //int id =  Noita.EntityLoad(@"data/entities/animals/boss_fish/fish_giga.xml", new Vector2(0, -200));

            //Logger.Instance.LogInformation($"ID: {id}");
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
