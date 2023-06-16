using System.Diagnostics;
using System.Runtime.CompilerServices;
using NoitaNET.API;
using NoitaNET.API.Logging;

namespace NoitaNET.TestMod;
internal class Benchmarks
{
    public Noita Noita;

    public Benchmarks(Noita noita)
    {
        Noita = noita;
    }

    public void BenchmarkPureRandomCalls(long n)
    {
        Stopwatch sw = Stopwatch.StartNew();

        double result = 1;
        for (long i = 0; i < n; i++)
        {
            //result = Noita.Random();
        }
        sw.Stop();

        Log(nameof(BenchmarkPureRandomCalls), n, sw);

        Consume(result);
    }

    public void BenchmarkGetPlayerByTag(long n)
    {
        Stopwatch sw = Stopwatch.StartNew();

        long result = 1;
        for (long i = 0; i < n; i++)
        {
            //result = Noita.EntityGetWithTag("player_unit")[0];
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
