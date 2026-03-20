using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;

namespace Maple.StrongId.ComparisonBenchmarks;

[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory, BenchmarkLogicalGroupRule.ByParams)]
[BenchmarkCategory("0")]
public class TestBench
{
    [Params(25_000)]
    public int Count { get; set; }

    [Benchmark(Baseline = true)]
    public int Maple_StrongId______()
    {
        // Baseline: round-trip a strongly-typed ID
        var id = new ItemTemplateId(Count);
        return (int)id;
    }
}
