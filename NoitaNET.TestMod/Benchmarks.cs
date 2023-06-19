using System.Diagnostics;
using System.Runtime.CompilerServices;
using NoitaNET.API;
using NoitaNET.API.Logging;
using NoitaNET.API.Noita;

namespace NoitaNET.TestMod;

internal class Benchmarks
{
    public EngineAPI RawEngineAPI;

    public Benchmarks(EngineAPI engineAPI)
    {
        RawEngineAPI = engineAPI;
    }

    public void BenchmarkPureRandomCalls(long n)
    {
        Stopwatch sw = Stopwatch.StartNew();

        double result = 1;
        for (long i = 0; i < n; i++)
        {
            RawEngineAPI.Random(out result);
        }
        sw.Stop();

        Log(nameof(BenchmarkPureRandomCalls), n, sw);

        Consume(result);
    }

    public void BenchmarkGetPlayerByTag(long n)
    {
        Stopwatch sw = Stopwatch.StartNew();

        nint result = 1;
        for (long i = 0; i < n; i++)
        {
            RawEngineAPI.EntityGetWithTag("player_unit", out nint[] results);
            result = results[0];
        }
        sw.Stop();

        Log(nameof(BenchmarkGetPlayerByTag), n, sw);

        Consume(result);
    }

    private void Log(string fn, long n, Stopwatch sw)
    {
        Logger.Instance.LogInformation($"[Benchmarks]: {fn}({n}) took {sw.Elapsed.TotalMilliseconds:F5}ms");
    }

    private object? __consumedObject;

    [MethodImpl(MethodImplOptions.NoInlining)]
    private void Consume(object? obj)
    {
        __consumedObject = obj;
    }
}
