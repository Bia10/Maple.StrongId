using BenchmarkDotNet.Attributes;

namespace Maple.StrongId.Benchmarks;

public class StrongIdBench
{
    [Benchmark(Baseline = true)]
    public int AccountId_Roundtrip()
    {
        var id = new AccountId(42);
        return (int)id;
    }

    [Benchmark]
    public int ItemTemplateId_IsHat()
    {
        var id = new ItemTemplateId(1002140);
        return id.IsHat ? 1 : 0;
    }
}
