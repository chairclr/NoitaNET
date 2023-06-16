using System.Diagnostics;
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

            // Benchmarks.BenchmarkPureRandomCalls(100_000_000);
            Benchmarks.BenchmarkGetPlayerByTag(100_000_000);
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
